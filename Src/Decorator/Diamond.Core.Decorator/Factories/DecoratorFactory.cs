//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Decorator
{
	/// <summary>
	/// Defines a generic repository factory that can be used to retrieve
	/// an object that implements IDecorator[TItem, TResult] from the container.
	/// </summary>
	public class DecoratorFactory : IDecoratorFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		public DecoratorFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="logger"></param>
		public DecoratorFactory(IServiceProvider serviceProvider, ILogger<DecoratorFactory> logger)
			: this(serviceProvider)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual ILogger<DecoratorFactory> Logger { get; set; } = new NullLogger<DecoratorFactory>();

		/// <summary>
		/// 
		/// </summary>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TDecoratedItem"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual Task<IDecorator<TDecoratedItem, TResult>> GetAsync<TDecoratedItem, TResult>(string name)
		{
			IDecorator<TDecoratedItem, TResult> returnValue = null;

			//
			// Get the decorator type being requested.
			//
			Type targetType = typeof(IDecorator<TDecoratedItem, TResult>);
			this.Logger.LogDebug("Finding a Decorator with key '{name}' and Target Type '{targetType}'.", name, targetType.Name);

			//
			// Get all decorators from the container of
			// type IDecorator<TParameter, TResult>.
			//
			IEnumerable<IDecorator> items = this.ServiceProvider.GetService<IEnumerable<IDecorator>>();
			IEnumerable<IDecorator> matchingItems = items.Where(t => t.Name == name);
			IEnumerable<IDecorator> nonMatchingItems = items.Except(matchingItems);
			this.Logger.LogDebug("{count} matching items of the target type were found.", matchingItems.Count());

			//
			// Within the list, find the target Decorator.
			//
			foreach (IDecorator item in matchingItems)
			{
				if (targetType.IsInstanceOfType(item))
				{
					returnValue = (IDecorator<TDecoratedItem, TResult>)item;
					this.Logger.LogDebug("The Decorator key '{name}' and Target Type '{targetType}' was found.", name, targetType.Name);
				}
				else
				{
					//
					// Dispose the item (if it supports it).
					//
					this.Logger.LogDebug("Attempting to dispose unused item '{item}'.", item.GetType().Name);
					item.TryDispose();
				}
			}

			//
			// Attempt to dispose the unused items.
			//
			foreach (IDecorator nonMatchingItem in nonMatchingItems)
			{
				this.Logger.LogDebug("Attempting to dispose non-matching item '{item}'.", nonMatchingItem.GetType().Name);
				nonMatchingItem.TryDispose();
			}

			//
			// Check the result.
			//
			if (returnValue == null)
			{
				this.Logger.LogDebug("The Decorator key '{name}' and Target Type '{targetType}' was NOT found. Throwing exception...", name, targetType.Name);
				throw new DecoratorNotFoundException<TDecoratedItem, TResult>(name);
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TDecoratedItem"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="name"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual async Task<IDecorator<TDecoratedItem, TResult>> GetAsync<TDecoratedItem, TResult>(string name, TDecoratedItem item)
		{
			IDecorator<TDecoratedItem, TResult> returnValue = null;

			//
			// Check the parameter.
			//
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			//
			// Get the decorator.
			//
			returnValue = await this.GetAsync<TDecoratedItem, TResult>(name);

			//
			// Set the instance.
			//
			returnValue.Item = item;

			return returnValue;
		}
	}
}

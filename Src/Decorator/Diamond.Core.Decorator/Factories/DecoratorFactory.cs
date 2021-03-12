//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
		{
			this.ServiceProvider = serviceProvider;
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		public ILogger<DecoratorFactory> Logger { get; set; } = new NullLogger<DecoratorFactory>();

		/// <summary>
		/// 
		/// </summary>
		protected IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TDecoratedItem"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public Task<IDecorator<TDecoratedItem, TResult>> GetAsync<TDecoratedItem, TResult>(string name)
			where TDecoratedItem : class
		{
			IDecorator<TDecoratedItem, TResult> returnValue = null;

			//
			// Get the decorator type being requested.
			//
			Type targetType = typeof(IDecorator<TDecoratedItem, TResult>);
			this.Logger.LogDebug("Finding an IDecorator of type '{targetType}' and container registration name of '{name}'.", targetType.Name, name);

			//
			// Get all decorators from the container of
			// type IDecorator<TItem>.
			//
			IEnumerable<IDecorator> items = this.ServiceProvider.GetService<IEnumerable<IDecorator>>();
			IDecorator decorator = items.Where(t => t.Name == name).FirstOrDefault();

			//
			// Within the list, find the target decorator.
			//
			if (decorator != null)
			{
				if (targetType.IsInstanceOfType(decorator))
				{
					this.Logger.LogDebug("IDecorator of type '{targetType}' and container registration name of '{name}' was found.", targetType.Name, name);
					returnValue = (IDecorator<TDecoratedItem, TResult>)decorator;
				}
				else
				{
					this.Logger.LogError("IDecorator of type '{targetType}' and container registration name of '{name}' was NOT found. Throwing exception...", targetType.Name, name);
					throw new DecoratorNotFoundException<TDecoratedItem, TResult>(name);
				}
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
		public async Task<IDecorator<TDecoratedItem, TResult>> GetAsync<TDecoratedItem, TResult>(string name, TDecoratedItem item)
			where TDecoratedItem : class
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

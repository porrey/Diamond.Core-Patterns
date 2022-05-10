//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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

namespace Diamond.Core.UnitOfWork
{
	/// <summary>
	/// This is a generic repository factory that can return a repository
	/// for any given entity interface.
	/// </summary>
	public class UnitOfWorkFactory : IUnitOfWorkFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		public UnitOfWorkFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="logger"></param>
		public UnitOfWorkFactory(IServiceProvider serviceProvider, ILogger<UnitOfWorkFactory> logger)
			: this(serviceProvider)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual ILogger<UnitOfWorkFactory> Logger { get; set; } = new NullLogger<UnitOfWorkFactory>();

		/// <summary>
		/// 
		/// </summary>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <typeparam name="TSourceItem"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual Task<IUnitOfWork<TResult, TSourceItem>> GetAsync<TResult, TSourceItem>(string name)
		{
			IUnitOfWork<TResult, TSourceItem> returnValue = null;

			//
			// Get the decorator type being requested.
			//
			Type targetType = typeof(IUnitOfWork<TResult, TSourceItem>);
			this.Logger.LogDebug("Finding a Unit of Work with key '{name}' and Target Type '{targetType}'.", name, targetType.Name);

			//
			// Get all decorators from the container of
			// type IUnitOfWork<TResult, TSourceItem>.
			//
			IEnumerable<IUnitOfWork> items = this.ServiceProvider.GetService<IEnumerable<IUnitOfWork>>();
			IEnumerable<IUnitOfWork> matchingItems = items.Where(t => t.Name == name);
			IEnumerable<IUnitOfWork> nonMatchingItems = items.Except(matchingItems);
			this.Logger.LogDebug("{count} matching items of the target type were found.", matchingItems.Count());

			//
			// Within the list, find the target unit of work.
			//
			foreach (IUnitOfWork item in matchingItems)
			{
				if (targetType.IsInstanceOfType(item))
				{
					returnValue = (IUnitOfWork<TResult, TSourceItem>)item;
					this.Logger.LogDebug("The Unit of Work key '{name}' and Target Type '{targetType}' was found.", name, targetType.Name);
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
			foreach (IUnitOfWork nonMatchingItem in nonMatchingItems)
			{
				this.Logger.LogDebug("Attempting to dispose non-matching item '{item}'.", nonMatchingItem.GetType().Name);
				nonMatchingItem.TryDispose();
			}

			//
			// Check the result.
			//
			if (returnValue == null)
			{
				this.Logger.LogDebug("The Unit of Work key '{name}' and Target Type '{targetType}' was NOT found. Throwing exception...", name, targetType.Name);
				throw new UnitOfWorkNotFoundException<TResult, TSourceItem>(name);
			}

			return Task.FromResult(returnValue);
		}
	}
}

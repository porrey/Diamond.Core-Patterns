//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
using System.Security.Authentication.ExtendedProtection;
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
		/// <param name="serviceKey">The container service key.</param>
		/// <returns></returns>
		public virtual Task<IUnitOfWork<TResult, TSourceItem>> GetAsync<TResult, TSourceItem>(string serviceKey)
		{
			IUnitOfWork<TResult, TSourceItem> returnValue = null;

			//
			// Validate the service key.
			//
			if (string.IsNullOrWhiteSpace(serviceKey))
			{
				this.Logger.LogDebug("The service key is null or whitespace.");
				throw new ArgumentException("The service key cannot be null or whitespace.", nameof(serviceKey));
			}

			//
			// Get all decorators from the container of
			// type IUnitOfWork<TResult, TSourceItem>.
			//
			this.Logger.LogDebug("Finding a Unit of Work with Service Key '{serviceKey}'.", serviceKey);
			IUnitOfWork item = this.ServiceProvider.GetKeyedService<IUnitOfWork>(serviceKey);

			if (item != null)
			{
				//
				// Get the decorator type being requested.
				//
				Type targetType = typeof(IUnitOfWork<TResult, TSourceItem>);

				if (targetType.IsInstanceOfType(item))
				{
					returnValue = (IUnitOfWork<TResult, TSourceItem>)item;
					this.Logger.LogDebug("The Unit of Work with Service Key '{serviceKey}' and Target Type '{targetType}' was found.", serviceKey, targetType.Name);
				}
				else
				{
					//
					// Dispose the item (if it supports it).
					//
					this.Logger.LogError("The Unit of Work with Service Key '{serviceKey}' was found, but it does not implement the Target Type '{targetType}'.", serviceKey, targetType.Name);
					item.TryDispose();
				}
			}
			else
			{
				this.Logger.LogDebug("The Unit of Work with Service Key '{serviceKey}' was NOT found.", serviceKey);
				throw new UnitOfWorkNotFoundException<TResult, TSourceItem>(serviceKey);
			}

			return Task.FromResult(returnValue);
		}
	}
}

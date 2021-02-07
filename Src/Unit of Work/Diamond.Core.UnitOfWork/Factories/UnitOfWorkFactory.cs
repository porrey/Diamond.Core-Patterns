// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
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
		public UnitOfWorkFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		public UnitOfWorkFactory(IServiceProvider serviceProvider, ILogger<UnitOfWorkFactory> logger)
		{
			this.ServiceProvider = serviceProvider;
			this.Logger = logger;
		}

		public ILogger<UnitOfWorkFactory> Logger { get; set; } = new NullLogger<UnitOfWorkFactory>();
		protected IServiceProvider ServiceProvider { get; set; }

		public Task<IUnitOfWork<TResult, TSourceItem>> GetAsync<TResult, TSourceItem>(string name)
		{
			IUnitOfWork<TResult, TSourceItem> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(IUnitOfWork<TResult, TSourceItem>);
			this.Logger.LogTrace($"Finding a Unit of Work with key '{name}' and Target Type '{targetType.Name}'.");

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IEnumerable<IUnitOfWork> items = this.ServiceProvider.GetService<IEnumerable<IUnitOfWork>>();
			IEnumerable<IUnitOfWork> keyItems = items.Where(t => t.Key == name);
			this.Logger.LogTrace($"{keyItems.Count()} match items of the target type were found.");

			// ***
			// *** Within the list, find the target decorator.
			// ***
			foreach (IUnitOfWork item in keyItems)
			{
				if (targetType.IsInstanceOfType(item))
				{
					returnValue = (IUnitOfWork<TResult, TSourceItem>)item;
					this.Logger.LogTrace($"The Unit of Work key '{name}' and Target Type '{targetType.Name}' was found.");
					break;
				}
			}

			// ***
			// *** Check the result.
			// ***
			if (returnValue == null)
			{
				this.Logger.LogTrace($"The Unit of Work key '{name}' and Target Type '{targetType.Name}' was NOT found. Throwing exception...");
				throw new UnitOfWorkNotFoundException<TResult, TSourceItem>(name);
			}

			return Task.FromResult(returnValue);
		}
	}
}

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
using Diamond.Core.Extensions.InterfaceInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Specification
{
	public class SpecificationFactory : ISpecificationFactory, ILoggerPublisher<SpecificationFactory>
	{
		public SpecificationFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		public SpecificationFactory(IServiceProvider serviceProvider, ILogger<SpecificationFactory> logger)
		{
			this.ServiceProvider = serviceProvider;
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		public ILogger<SpecificationFactory> Logger { get; set; } = new NullLogger<SpecificationFactory>();

		/// <summary>
		/// 
		/// </summary>
		protected IServiceProvider ServiceProvider { get; set; }

		public Task<ISpecification<TResult>> GetAsync<TResult>(string name)
		{
			ISpecification<TResult> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(ISpecification<TResult>);
			this.Logger.LogTrace($"Finding a Specification with container registration name '{name}' and Target Type '{targetType.Name}'.");

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IEnumerable<ISpecification> items = this.ServiceProvider.GetService<IEnumerable<ISpecification>>();
			ISpecification item = items.Where(t => t.Name == name).SingleOrDefault();

			// ***
			// *** Within the list, find the target decorator.
			// ***
			if (item != null)
			{
				if (targetType.IsInstanceOfType(item))
				{
					this.Logger.LogTrace($"The Specification '{name}' and Target Type '{targetType.Name}' was found.");
					returnValue = (ISpecification<TResult>)item;
				}
				else
				{
					this.Logger.LogError($"The Specification key '{name}' and Target Type '{targetType.Name}' was NOT found. Throwing exception...");
					throw new SpecificationNotFoundException<TResult>(name);
				}
			}
			else
			{
				this.Logger.LogError($"The Specification key '{name}' was NOT found. Throwing exception...");
				throw new SpecificationNotFoundException<TResult>(name);
			}

			return Task.FromResult(returnValue);
		}

		public Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>(string name)
		{
			ISpecification<TParameter, TResult> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(ISpecification<TParameter, TResult>);
			this.Logger.LogTrace($"Finding a Specification with container registration name '{name}' and Target Type '{targetType.Name}'.");

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IEnumerable<ISpecification> items = this.ServiceProvider.GetService<IEnumerable<ISpecification>>();
			ISpecification item = items.Where(t => t.Name == name).SingleOrDefault();

			// ***
			// *** Within the list, find the target decorator.
			// ***
			if (item != null)
			{
				if (targetType.IsInstanceOfType(item))
				{
					this.Logger.LogTrace($"The Specification '{name}' and Target Type '{targetType.Name}' was found.");
					returnValue = (ISpecification<TParameter, TResult>)item;
				}
				else
				{
					this.Logger.LogError($"The Specification key '{name}' and Target Type '{targetType.Name}' was NOT found. Throwing exception...");
					throw new SpecificationNotFoundException<TParameter, TResult>(name);
				}
			}
			else
			{
				this.Logger.LogError($"The Specification key '{name}' was NOT found. Throwing exception...");
				throw new SpecificationNotFoundException<TParameter, TResult>(name);
			}

			return Task.FromResult(returnValue);
		}
	}
}

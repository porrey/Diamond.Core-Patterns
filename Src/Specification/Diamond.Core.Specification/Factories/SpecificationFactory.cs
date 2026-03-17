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
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Specification
{
	/// <summary>
	/// Provides a factory that can be used to retrieve a specific instance of
	/// <see cref="ISpecification"/> from a container. The scope is always 
	/// controlled by the registration of the specification into the container.
	/// </summary>
	public class SpecificationFactory : ISpecificationFactory
	{
		/// <summary>
		/// Creates an instance of <see cref="SpecificationFactory"/> using the specified <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="serviceProvider"></param>
		public SpecificationFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// Creates an instance of <see cref="SpecificationFactory"/> using the specified <see cref="IServiceProvider"/>
		/// and logger.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="logger"></param>
		public SpecificationFactory(ILogger<SpecificationFactory> logger, IServiceProvider serviceProvider)
			: this(serviceProvider)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Gets/sets the instance of the logger used by the factory. The default is a null logger.
		/// </summary>
		public virtual ILogger<SpecificationFactory> Logger { get; set; } = new NullLogger<SpecificationFactory>();

		/// <summary>
		/// Gets/sets the <see cref="IServiceProvider"/> used by the factory to retrieve 
		/// the specification instances.
		/// </summary>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="serviceKey">The container service key.</param>
		/// <returns></returns>
		public virtual Task<ISpecification<TResult>> GetAsync<TResult>(string serviceKey)
		{
			ISpecification<TResult> returnValue = null;

			//
			// Validate the service key.
			//
			if (string.IsNullOrWhiteSpace(serviceKey))
			{
				this.Logger.LogDebug("The service key is null or whitespace.");
				throw new ArgumentException("The service key cannot be null or whitespace.", nameof(serviceKey));
			}

			//
			// Get the specification.
			//
			this.Logger.LogDebug("Finding a Specification with key '{name}'.", serviceKey);
			ISpecification item = this.ServiceProvider.GetKeyedService<ISpecification>(serviceKey);

			if ((item != null))
			{
				//
				// Get the specification type being requested.
				//
				Type targetType = typeof(ISpecification<TResult>);

				if (targetType.IsInstanceOfType(item))
				{
					returnValue = (ISpecification<TResult>)item;
					this.Logger.LogDebug("The Specification with Service Key '{serviceKey}' and Target Type '{targetType}' was found.", serviceKey, targetType.Name);
				}
				else
				{
					//
					// Dispose the item (if it supports it).
					//
					this.Logger.LogError("The Specification with Service Key '{serviceKey}' was found but is NOT of the expected type '{targetType}'. Attempting to dispose the item.", serviceKey, targetType.Name);
					item.TryDispose();
				}
			}
			else
			{
				this.Logger.LogDebug("The Specification with Service Key '{serviceKey}' was NOT found.", serviceKey);
				throw new SpecificationNotFoundException<TResult>(serviceKey);
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TParameter"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="serviceKey">The container service key.</param>
		/// <returns></returns>
		public virtual Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>(string serviceKey)
		{
			ISpecification<TParameter, TResult> returnValue = null;

			//
			// Validate the service key.
			//
			if (string.IsNullOrWhiteSpace(serviceKey))
			{
				this.Logger.LogDebug("The service key is null or whitespace.");
				throw new ArgumentException("The service key cannot be null or whitespace.", nameof(serviceKey));
			}

			//
			// Get the specification type being requested.
			//
			this.Logger.LogDebug("Finding a Specification with key '{key}'.", serviceKey);
			ISpecification item = this.ServiceProvider.GetKeyedService<ISpecification>(serviceKey);

			if ((item != null))
			{
				//
				// Get the specification type being requested.
				//
				Type targetType = typeof(ISpecification<TParameter, TResult>);

				if (targetType.IsInstanceOfType(item))
				{
					returnValue = (ISpecification<TParameter, TResult>)item;
					this.Logger.LogDebug("The Specification with Service Key '{serviceKey}' and Target Type '{targetType}' was found.", serviceKey, targetType.Name);
				}
				else
				{
					//
					// Dispose the item (if it supports it).
					//
					this.Logger.LogError("The Specification with Service Key '{serviceKey}' was found but is NOT of the expected type '{targetType}'. Attempting to dispose the item.", serviceKey, targetType.Name);
					item.TryDispose();
				}
			}
			else
			{
				this.Logger.LogDebug("The Specification with Service Key '{serviceKey}' was NOT found.", serviceKey);
				throw new SpecificationNotFoundException<TParameter, TResult>(serviceKey);

			}

			return Task.FromResult(returnValue);
		}
	}
}

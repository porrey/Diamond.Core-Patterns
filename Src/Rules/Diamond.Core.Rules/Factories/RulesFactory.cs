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

namespace Diamond.Core.Rules
{
	/// <summary>
	/// Defines a generic repository factory that can be used to retrieve
	/// an object that implements <see cref="IRule"/> from the container.
	/// </summary>
	public class RulesFactory : IRulesFactory
	{
		/// <summary>
		/// Creates an instance of <see cref="IRule"/> with the
		/// specified instance of <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="serviceProvider"></param>
		public RulesFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="logger"></param>
		public RulesFactory(IServiceProvider serviceProvider, ILogger<RulesFactory> logger)
			: this(serviceProvider)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Gets/sets the internal instance of <see cref="IServiceProvider"/>.
		/// </summary>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// Gets/sets a reference to the <see cref="ILogger"/> for this object.
		/// </summary>
		public virtual ILogger<RulesFactory> Logger { get; set; } = new NullLogger<RulesFactory>();

		/// <summary>
		/// Get all model rule instances registered based on TInterface.
		/// </summary>
		/// <typeparam name="TItem">The type of the model being validated.</typeparam>
		/// <returns>A list of <see cref="IRule"/> instances.</returns>
		public virtual Task<IEnumerable<IRule<TItem>>> GetAllAsync<TItem>()
		{
			return this.GetAllAsync<TItem>(null);
		}

		/// <summary>
		/// Get all model rule instances registered based on TInterface and group name.
		/// </summary>
		/// <typeparam name="TItem">The type of the model being validated.</typeparam>
		/// <returns>A list of <see cref="IRule"/> instances.</returns>
		public virtual Task<IEnumerable<IRule<TItem>>> GetAllAsync<TItem>(string group)
		{
			IList<IRule<TItem>> returnValue = new List<IRule<TItem>>();

			//
			// Get the decorator type being requested.
			//
			Type targetType = typeof(IRule<TItem>);

			//
			// Get all decorators from the container of
			// type IDecorator<TItem>.
			//
			IEnumerable<IRule> items = this.ServiceProvider.GetKeyedService<IEnumerable<IRule>>(group);

			if (items.Any())
			{
				foreach (IRule item in items)
				{
					if (targetType.IsInstanceOfType(item))
					{
						returnValue.Add((IRule<TItem>)item);
					}
					else
					{
						//
						// Dispose the item (if it supports it). This should be rare but will
						// happen if the developer mis-configures the app.
						//
						this.Logger.LogDebug("Attempting to dispose unused item '{item}'.", item.GetType().Name);
						item.TryDispose();
					}
				}
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(group))
				{
					throw new RulesNotFoundException<TItem>(group);
				}
				else
				{
					throw new RulesNotFoundException<TItem>();
				}
			}

			return Task.FromResult<IEnumerable<IRule<TItem>>>(returnValue);
		}

		/// <summary>
		/// Get all model rule instances registered based on TInterface and group name.
		/// </summary>
		/// <typeparam name="TItem">The type of the model being validated.</typeparam>
		/// <typeparam name="TResult">The type of the model being validated.</typeparam>
		/// <returns>A list of <see cref="IRule"/> instances.</returns>
		public virtual Task<IEnumerable<IRule<TItem, TResult>>> GetAllAsync<TItem, TResult>()
		{
			return this.GetAllAsync<TItem, TResult>(null);
		}

		/// <summary>
		/// Get all model rule instances registered based on TInterface and group name.
		/// </summary>
		/// <typeparam name="TItem">The type of the model being validated.</typeparam>
		/// <typeparam name="TResult">The type of the model being validated.</typeparam>
		/// <returns>A list of <see cref="IRule"/> instances.</returns>
		public virtual Task<IEnumerable<IRule<TItem, TResult>>> GetAllAsync<TItem, TResult>(string serviceKey)
		{
			IList<IRule<TItem, TResult>> returnValue = [];

			//
			// Validate the service key.
			//
			if (string.IsNullOrWhiteSpace(serviceKey))
			{
				this.Logger.LogDebug("The service key is null or whitespace.");
				throw new ArgumentException("The service key cannot be null or whitespace.", nameof(serviceKey));
			}

			//
			// Get all rules from the container of type IRule with the specified service key.
			//
			IEnumerable<IRule> items = this.ServiceProvider.GetKeyedService<IEnumerable<IRule>>(serviceKey);

			if (items.Any())
			{
				this.Logger.LogDebug("{count} Rules with Service Key '{serviceKey}' were found.", items.Count(), serviceKey);

				//
				// Get the decorator type being requested.
				//
				Type targetType = typeof(IRule<TItem, TResult>);
				this.Logger.LogDebug("Filtering Rules with Target Type '{name}'.", targetType.Name);

				foreach (IRule item in items)
				{
					if (targetType.IsInstanceOfType(item))
					{
						returnValue.Add((IRule<TItem, TResult>)item);
					}
					else
					{
						//
						// Dispose the item (if it supports it). This should be rare but will
						// happen if the developer mis-configures the app.
						//
						this.Logger.LogError("Item '{item}' is not of the expected type '{name}'. Attempting to dispose.", item.GetType().Name, targetType.Name);
						item.TryDispose();
					}
				}
			}
			else
			{
				this.Logger.LogDebug("No Rules were found with Service Key '{serviceKey}'.", serviceKey);
				throw new RulesNotFoundException<TItem>(serviceKey);
			}

			return Task.FromResult<IEnumerable<IRule<TItem, TResult>>>(returnValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="serviceKey">The container service key.</param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual async Task<string> EvaluateAsync<TItem>(string serviceKey, TItem item)
		{
			string returnValue = string.Empty;

			IEnumerable<IRule<TItem, IRuleResult>> rules = await this.GetAllAsync<TItem, IRuleResult>(serviceKey);

			//
			// Execute the specification to get the list of qualified widgets.
			//
			IEnumerable<IRuleResult> results = [.. rules.Select(t => t.ValidateAsync(item).Result)];

			//
			// Compile a list of messages.
			//
			IEnumerable<IRuleResult> messages = [.. results.Where(t => !t.Passed)];

			//
			// Join the errors into a single string.
			//
			returnValue = string.Join(" ", messages.Select(t => t.ErrorMessage));

			return returnValue;
		}
	}
}

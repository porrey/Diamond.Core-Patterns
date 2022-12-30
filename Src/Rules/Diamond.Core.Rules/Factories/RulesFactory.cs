//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
			IEnumerable<IRule> items = this.ServiceProvider.GetService<IEnumerable<IRule>>();

			IEnumerable<IRule> groupItems = Array.Empty<IRule>();
			if (!string.IsNullOrEmpty(group))
			{
				groupItems = items.Where(t => t.Group == group);
			}

			//
			// Get the non matching items.
			//
			IEnumerable<IRule> nonMatchingItems = items.Except(groupItems);

			//
			// Attempt to dispose the unused items.
			//
			foreach (IRule nonMatchingItem in nonMatchingItems)
			{
				this.Logger.LogDebug("Attempting to dispose non-matching item '{item}'.", nonMatchingItem.GetType().Name);
				nonMatchingItem.TryDispose();
			}

			if (groupItems.Any())
			{
				foreach (IRule item in groupItems)
				{
					if (targetType.IsInstanceOfType(item))
					{
						returnValue.Add((IRule<TItem>)item);
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
		public virtual Task<IEnumerable<IRule<TItem, TResult>>> GetAllAsync<TItem, TResult>(string group)
		{
			IList<IRule<TItem, TResult>> returnValue = new List<IRule<TItem, TResult>>();

			//
			// Get the decorator type being requested.
			//
			Type targetType = typeof(IRule<TItem, TResult>);
			this.Logger.LogDebug("Finding Rules with group '{group}' and Target Type '{name}'.", group, targetType.Name);

			//
			// Get all decorators from the container of
			// type IDecorator<TItem>.
			//
			IEnumerable<IRule> items = this.ServiceProvider.GetService<IEnumerable<IRule>>();

			IEnumerable<IRule> groupItems = Array.Empty<IRule>();
			if (!string.IsNullOrEmpty(group))
			{
				groupItems = items.Where(t => t.Group == group);
			}

			//
			// Get the non matching items.
			//
			IEnumerable<IRule> nonMatchingItems = items.Except(groupItems);

			//
			// Attempt to dispose the unused items.
			//
			foreach (IRule nonMatchingItem in nonMatchingItems)
			{
				this.Logger.LogDebug("Attempting to dispose non-matching item '{item}'.", nonMatchingItem.GetType().Name);
				nonMatchingItem.TryDispose();
			}

			if (items.Any())
			{
				this.Logger.LogDebug("{count} Rules with group '{group}' and Target Type '{targetType}' were found.", items.Count(), group, targetType.Name);

				foreach (IRule item in items)
				{
					if (targetType.IsInstanceOfType(item))
					{
						returnValue.Add((IRule<TItem, TResult>)item);
					}
					else
					{
						item.TryDispose();
					}
				}
			}
			else
			{
				this.Logger.LogDebug("No Rules were found with group '{group}' and Target Type '{targetType.Name}'. Throwing exception...", group);

				if (!string.IsNullOrWhiteSpace(group))
				{
					throw new RulesNotFoundException<TItem>(group);
				}
				else
				{
					throw new RulesNotFoundException<TItem>();
				}
			}

			return Task.FromResult<IEnumerable<IRule<TItem, TResult>>>(returnValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="group"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual async Task<string> EvaluateAsync<TItem>(string group, TItem item)
		{
			string returnValue = string.Empty;

			this.Logger.LogDebug("Retrieving rules to validate shipment.");
			IEnumerable<IRule<TItem, IRuleResult>> rules = await this.GetAllAsync<TItem, IRuleResult>(group);

			//
			// Execute the specification to get the list of qualified widgets.
			//
			this.Logger.LogDebug("Executing rules on shipment.");
			IEnumerable<IRuleResult> results = rules.Select(t => t.ValidateAsync(item).Result).ToArray();

			//
			// Compile a list of messages.
			//
			IEnumerable<IRuleResult> messages = results.Where(t => !t.Passed).ToArray();

			//
			// Join the errors into a single string.
			//
			returnValue = string.Join(" ", messages.Select(t => t.ErrorMessage));

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<string> EvaluateAsync<TItem>(TItem item)
		{
			return this.EvaluateAsync(null, item);
		}
	}
}

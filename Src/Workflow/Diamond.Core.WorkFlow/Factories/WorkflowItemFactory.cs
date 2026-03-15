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

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// 
	/// </summary>
	public class WorkflowItemFactory : IWorkflowItemFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		public WorkflowItemFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="logger"></param>
		public WorkflowItemFactory(IServiceProvider serviceProvider, ILogger<WorkflowItemFactory> logger)
			: this(serviceProvider)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual ILogger<WorkflowItemFactory> Logger { get; set; } = new NullLogger<WorkflowItemFactory>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceKey">The container service key.</param>
		/// <returns></returns>
		public virtual Task<IEnumerable<IWorkflowItem>> GetItemsAsync(string serviceKey)
		{
			IList<IWorkflowItem> returnValue = [];

			//
			// Find the repository that supports the given type.
			//
			IEnumerable<IWorkflowItem> items = this.ServiceProvider.GetKeyedService<IEnumerable<IWorkflowItem>>(serviceKey);
			this.Logger.LogDebug("Found {count} workflow items for Service Key '{serviceKey}'.", items.Count(), serviceKey);

			if (items.Any())
			{
				this.Logger.LogDebug("Loading workflow items for Service Key '{serviceKey}'.", serviceKey);

				//
				// Get the type being requested.
				//
				Type targetType = typeof(IWorkflowItem);

				foreach (IWorkflowItem item in items)
				{
					if (targetType.IsInstanceOfType(item))
					{
						returnValue.Add((IWorkflowItem)item);
						this.Logger.LogDebug("Added workflow item '{name}'.", item.Name);
					}
					else
					{
						this.Logger.LogDebug("Skipping workflow item '{name}' because it does not have the correct Type.", item.Name);

						//
						// Dispose the item (if it supports it).
						//
						this.Logger.LogDebug("Attempting to dispose unused item '{name}'.", item.Name);
						item.TryDispose();
					}
				}
			}
			else
			{
				//
				// No items
				//
				this.Logger.LogError("Work flow items for Service Key '{serviceKey}' have not been configured.", serviceKey);
				throw new Exception($"Work flow items for Service Key '{serviceKey}' have not been configured.");
			}

			return Task.FromResult<IEnumerable<IWorkflowItem>>(returnValue);
		}
	}
}

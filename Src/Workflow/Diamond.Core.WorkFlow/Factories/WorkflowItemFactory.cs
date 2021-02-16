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
using Diamond.Core.Extensions.InterfaceInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// 
	/// </summary>
	public class WorkflowItemFactory : IWorkflowItemFactory, ILoggerPublisher<WorkflowItemFactory>
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
		{
			this.ServiceProvider = serviceProvider;
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ILogger<WorkflowItemFactory> Logger { get; set; } = new NullLogger<WorkflowItemFactory>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public Task<IEnumerable<IWorkflowItem>> GetItemsAsync(string groupName)
		{
			IList<IWorkflowItem> returnValue = new List<IWorkflowItem>();

			//
			// Get the type being requested.
			//
			Type targetType = typeof(IWorkflowItem);

			//
			// Find the repository that supports the given type.
			//
			IEnumerable<IWorkflowItem> items = this.ServiceProvider.GetService<IEnumerable<IWorkflowItem>>();
			IEnumerable<IWorkflowItem> groupItems = items.Where(t => t.Group == groupName);
			this.Logger.LogDebug($"Found {groupItems.Count()} workflow items for group '{groupName}'.");

			if (groupItems.Count() > 0)
			{
				this.Logger.LogDebug($"Loading workflow items for group '{groupName}'.");

				foreach (IWorkflowItem groupItem in groupItems)
				{
					if (targetType.IsInstanceOfType(groupItem))
					{
						returnValue.Add((IWorkflowItem)groupItem);
						this.Logger.LogDebug($"Added workflow item '{groupItem.Name}'.");
					}
					else
					{
						this.Logger.LogDebug($"Skipping workflow item '{groupItem.Name}' because it does not have the correct Type.");
					}
				}
			}
			else
			{
				//
				// No items
				//
				this.Logger.LogError($"Work flow items for group '{groupName}' have not been configured.");
				throw new Exception($"Work flow items for group '{groupName}' have not been configured.");
			}

			return Task.FromResult<IEnumerable<IWorkflowItem>>(returnValue);
		}
	}
}

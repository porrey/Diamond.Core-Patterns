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
using Diamond.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.WorkFlow
{
	/// <summary>
	/// 
	/// </summary>
	public class WorkFlowItemFactory : IWorkFlowItemFactory, ILoggerPublisher<WorkFlowItemFactory>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		public WorkFlowItemFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ILogger<WorkFlowItemFactory> Logger { get; set; } = new NullLogger<WorkFlowItemFactory>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public Task<IEnumerable<IWorkFlowItem>> GetItemsAsync(string groupName)
		{
			IList<IWorkFlowItem> returnValue = new List<IWorkFlowItem>();

			// ***
			// *** Get the type being requested.
			// ***
			Type targetType = typeof(IWorkFlowItem);

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IEnumerable<IWorkFlowItem> items = this.ServiceProvider.GetService<IEnumerable<IWorkFlowItem>>();
			IEnumerable<IWorkFlowItem> groupItems = items.Where(t => t.Group == groupName);
			this.Logger.LogTrace($"Found {groupItems.Count()} Work-Flow items for group '{groupName}'.");

			if (groupItems.Count() > 0)
			{
				this.Logger.LogTrace($"Loading Work-Flow items for group '{groupName}'.");
				
				foreach (IWorkFlowItem groupItem in groupItems)
				{
					if (targetType.IsInstanceOfType(groupItem))
					{
						returnValue.Add((IWorkFlowItem)groupItem);
						this.Logger.LogTrace($"Added Work-Flow item '{groupItem.Name}'.");
					}
					else
					{
						this.Logger.LogTrace($"Skipping Work-Flow item '{groupItem.Name}' because it does not have the correct Type.");
					}
				}
			}
			else
			{
				// ***
				// *** No items
				// ***
				this.Logger.LogError($"Work flow items for group '{groupName}' have not been configured.");
				throw new Exception($"Work flow items for group '{groupName}' have not been configured.");
			}

			return Task.FromResult<IEnumerable<IWorkFlowItem>>(returnValue);
		}
	}
}

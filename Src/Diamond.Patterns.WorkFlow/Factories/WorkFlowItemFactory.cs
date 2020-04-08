// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
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
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	public class WorkFlowItemFactory : IWorkFlowItemFactory
	{
		public WorkFlowItemFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		protected IObjectFactory ObjectFactory { get; set; }

		public Task<IEnumerable<IWorkFlowItem<TContextDecorator, TContext>>> GetItemsAsync<TContextDecorator, TContext>(string groupName)
			where TContext : IContext
			where TContextDecorator : IContextDecorator<TContext>
		{
			IList<IWorkFlowItem<TContextDecorator, TContext>> returnValue = new List<IWorkFlowItem<TContextDecorator, TContext>>();

			// ***
			// *** Get the type being requested.
			// ***
			Type targetType = typeof(IWorkFlowItem<TContextDecorator, TContext>);

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IEnumerable<IWorkFlowItem> items = this.ObjectFactory.GetAllInstances<IWorkFlowItem>();
			IEnumerable<IWorkFlowItem> groupItems = items.Where(t => t.Group == groupName);

			if (groupItems.Count() > 0)
			{
				foreach (IWorkFlowItem groupItem in groupItems)
				{
					if (targetType.IsInstanceOfType(groupItem))
					{
						returnValue.Add((IWorkFlowItem<TContextDecorator, TContext>)groupItem);
					}
				}
			}
			else
			{
				// ***
				// *** No items
				// ***
				throw new Exception($"Work flow items for group '{groupName}' have not been configured.");
			}

			return Task.FromResult<IEnumerable<IWorkFlowItem<TContextDecorator, TContext>>>(returnValue);
		}
	}
}

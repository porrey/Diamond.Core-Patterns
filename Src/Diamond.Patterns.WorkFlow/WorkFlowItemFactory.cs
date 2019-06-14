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

		public Task<IEnumerable<IWorkFlowItem<TContext>>> GetItemsAsync<TContext>(string group) where TContext : IContext
		{
			IEnumerable<IWorkFlowItem<TContext>> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(TContext);

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IEnumerable<IWorkFlowItem<TContext>> items = this.ObjectFactory.GetAllInstances<IWorkFlowItem<TContext>>();

			returnValue = items.Where(t => t.Group == group).ToArray();

			return Task.FromResult(returnValue);
		}
	}
}

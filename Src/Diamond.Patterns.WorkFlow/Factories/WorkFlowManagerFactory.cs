using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	/// <summary>
	/// This is a generic repository factory that can return a repository
	/// for any given entity interface.
	/// </summary>
	public class WorkFlowManagerFactory : IWorkFlowManagerFactory
	{
		public WorkFlowManagerFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		protected IObjectFactory ObjectFactory { get; set; }

		public Task<IWorkFlowManager<TContextDecorator, TContext>> GetAsync<TContextDecorator, TContext>(string groupName)
			where TContextDecorator : IContextDecorator<TContext>
			where TContext : IContext
		{
			IWorkFlowManager<TContextDecorator, TContext> returnValue = null;

			// ***
			// *** Get the type being requested.
			// ***
			Type targetType = typeof(IWorkFlowManager<TContextDecorator, TContext>);

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IEnumerable<IWorkFlowManager> items = this.ObjectFactory.GetAllInstances<IWorkFlowManager>();
			IWorkFlowManager item = items.Where(t => t.Group == groupName).SingleOrDefault();

			if (item != null)
			{
				returnValue = (IWorkFlowManager<TContextDecorator, TContext>)item;
			}
			else
			{
				throw new WorkFlowManagerNotFoundException<TContextDecorator, TContext>(groupName);
			}

			return Task.FromResult(returnValue);
		}
	}
}

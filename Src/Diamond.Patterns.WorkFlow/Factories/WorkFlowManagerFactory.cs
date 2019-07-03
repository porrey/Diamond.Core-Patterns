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

		public Task<IWorkFlowManager<TContext>> GetAsync<TContext>(string groupName) where TContext : IContext
		{
			IWorkFlowManager<TContext> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IEnumerable<IWorkFlowManager<TContext>> items = this.ObjectFactory.GetAllInstances<IWorkFlowManager<TContext>>();
			returnValue = items.Where(t => t.Group == groupName).SingleOrDefault();

			return Task.FromResult(returnValue);
		}
	}
}

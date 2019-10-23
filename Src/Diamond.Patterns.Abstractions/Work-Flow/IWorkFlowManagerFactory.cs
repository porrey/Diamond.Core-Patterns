using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWorkFlowManagerFactory
	{
		Task<IWorkFlowManager<TContextDecorator, TContext>> GetAsync<TContextDecorator, TContext>(string groupName)
			where TContext : IContext
			where TContextDecorator : IContextDecorator<TContext>;
	}
}

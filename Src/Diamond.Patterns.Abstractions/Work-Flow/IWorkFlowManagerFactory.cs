using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWorkFlowManagerFactory
	{
		Task<IWorkFlowManager<TContext>> GetAsync<TContext>(string groupName) where TContext : IContext;
	}
}

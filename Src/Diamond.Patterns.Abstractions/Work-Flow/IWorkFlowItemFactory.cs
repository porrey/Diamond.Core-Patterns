using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWorkFlowItemFactory
	{
		Task<IEnumerable<IWorkFlowItem<TContext>>> GetItemsAsync<TContext>(string key) where TContext : IContext;
	}
}

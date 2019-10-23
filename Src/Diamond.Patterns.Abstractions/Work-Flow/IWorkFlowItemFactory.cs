using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWorkFlowItemFactory
	{
		Task<IEnumerable<IWorkFlowItem<TContextDecorator, TContext>>> GetItemsAsync<TContextDecorator, TContext>(string key)
			where TContext : IContext
			where TContextDecorator : IContextDecorator<TContext>;
	}
}

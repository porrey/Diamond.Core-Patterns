using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWorkFlowManager<TContext>  where TContext : IContext
	{
		IWorkFlowItem<TContext>[] Steps { get; }
		string Group { get; }
		Task<bool> ExecuteWorkflowAsync(IContextDecorator<TContext> context);
	}
}
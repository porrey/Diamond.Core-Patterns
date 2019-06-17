using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWorkFlowManager
	{
	}

	public interface IWorkFlowManager<TContext> : IWorkFlowManager where TContext : IContext
	{
		IWorkFlowItem<TContext>[] Steps { get; }
		string Group { get; }
		Task<bool> ExecuteWorkflowAsync(IContextDecorator<TContext> context);
	}
}
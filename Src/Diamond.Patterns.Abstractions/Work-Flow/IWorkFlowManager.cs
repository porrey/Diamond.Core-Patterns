using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWorkFlowManager
	{
		string Group { get; }
	}

	public interface IWorkFlowManager<TContextDecorator, TContext> : IWorkFlowManager
		where TContext : IContext
		where TContextDecorator : IContextDecorator<TContext>
	{
		IWorkFlowItem<TContextDecorator, TContext>[] Steps { get; }
		Task<bool> ExecuteWorkflowAsync(TContextDecorator context);
	}
}
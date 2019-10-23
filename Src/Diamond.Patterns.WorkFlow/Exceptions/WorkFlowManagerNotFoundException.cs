using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	public class WorkFlowManagerNotFoundException<TContextDecorator, TContext> : DiamondPatternsException
	{
		public WorkFlowManagerNotFoundException(string groupName)
			: base($"A decorator of type 'IWorkFlowManager<{typeof(TContextDecorator).Name}, {typeof(TContext).Name}>' with group name '{groupName}' has not been configured.")
		{
		}
	}
}

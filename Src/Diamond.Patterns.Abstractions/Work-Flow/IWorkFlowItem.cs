using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWorkFlowItem<TContext> where TContext : IContext
	{
		int Ordinal { get; set; }
		string Name { get; }
		string Group { get; }
		Task<bool> ExecuteStepAsync(IContextDecorator<TContext> context);
	}
}

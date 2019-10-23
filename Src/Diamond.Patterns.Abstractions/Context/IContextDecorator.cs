using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IContextDecorator<TContext> where TContext : IContext
	{
		TContext Item { get; set; }
		Task ResetAsync();
		IStateDictionary Properties { get; set; }
	}
}

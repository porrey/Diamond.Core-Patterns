using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IDecorator
	{
	}

	public interface IDecorator<TItem, TResult> : IDecorator
	{
		Task<TResult> TakeActionAsync(TItem item);
	}
}

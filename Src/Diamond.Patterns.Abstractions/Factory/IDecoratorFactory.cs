using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IDecoratorFactory
	{
		Task<IDecorator<TItem, TResult>> GetAsync<TItem, TResult>();
		Task<IDecorator<TItem, TResult>> GetAsync<TItem, TResult>(string name);
	}
}

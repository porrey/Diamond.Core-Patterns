using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface ISpecificationFactory
	{
		Task<ISpecification<TResult>> GetAsync<TResult>();
		Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>();
	}
}

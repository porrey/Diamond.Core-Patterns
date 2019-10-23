using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface ISpecificationFactory
	{
		Task<ISpecification<TResult>> GetAsync<TResult>();
		Task<ISpecification<TResult>> GetAsync<TResult>(string name);
		Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>();
		Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>(string name);
	}
}

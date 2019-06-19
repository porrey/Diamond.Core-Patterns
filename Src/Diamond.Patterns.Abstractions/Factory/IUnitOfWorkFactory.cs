using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IUnitOfWorkFactory
	{
		Task<IUnitOfWork<TResult, TSourceItem>> GetAsync<TResult, TSourceItem>();
	}
}

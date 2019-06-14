using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IUnitOfWorkFactory
	{
		Task<IUnitOfWork<TSourceItem>> GetAsync<TSourceItem>();
	}
}

using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IUnitOfWork<TSourceItem>
	{
		Task<bool> Commit(TSourceItem item);
	}
}

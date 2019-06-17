using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IUnitOfWork
	{
	}

	public interface IUnitOfWork<TSourceItem> : IUnitOfWork
	{
		Task<bool> Commit(TSourceItem item);
	}
}

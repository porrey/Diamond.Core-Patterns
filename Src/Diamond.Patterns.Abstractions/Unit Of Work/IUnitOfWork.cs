using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IUnitOfWork
	{
	}

	public interface IUnitOfWork<TResult, TSourceItem> : IUnitOfWork
	{
		Task<TResult> Commit(TSourceItem item);
	}
}

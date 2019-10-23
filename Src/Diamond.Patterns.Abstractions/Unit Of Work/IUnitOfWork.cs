using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IUnitOfWork
	{
		string Key { get; }
	}

	public interface IUnitOfWork<TResult, TSourceItem> : IUnitOfWork
	{
		Task<TResult> CommitAsync(TSourceItem item);
	}
}

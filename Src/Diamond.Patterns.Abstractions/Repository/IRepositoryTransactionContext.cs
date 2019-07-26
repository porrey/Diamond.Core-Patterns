using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IRepositoryTransactionContext
	{
		Task CommitTransactionAsync();
		Task RollbackTransactionAsync();
	}
}

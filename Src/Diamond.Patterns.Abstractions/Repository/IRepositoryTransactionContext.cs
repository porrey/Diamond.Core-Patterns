using System;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IRepositoryTransactionContext : IDisposable
	{
		Task CommitTransactionAsync();
		Task RollbackTransactionAsync();
	}
}

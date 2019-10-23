using System;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public enum ContextIsolationLevel
	{
		/// <summary>
		/// Protects against Lost Updates, Dirty Reads, non-repeatable Reads, and Phantoms
		/// </summary>
		RepeatableRead,
		/// <summary>
		/// Protects against Lost Updates, Dirty Reads, and non-repeatable Reads. Read stability
		/// does not protect against Phantoms.
		/// </summary>
		ReadStability,
		/// <summary>
		/// Protects against non-repeatable Reads and Phantoms. Cursor Stability does not protect
		/// against Lost Updates and Dirty Reads.
		/// </summary>
		CursorStability,
		/// <summary>
		/// Protects against Lost Updates. Uncommitted Read does not protect against Phantoms,
		/// Dirty Reads, and Non-repeatable Reads.
		/// </summary>
		UncommitedRead,
	}

	/// <summary>
	/// This interface is used as a handle for any type of context
	/// without the need to expose the type.
	/// </summary>
	public interface IRepositoryContext : IDisposable
	{
		Task EnableBulkLoadAsync();
		Task DisableBulkLoadAsync();
		Task<int> ExecuteSqlCommandAsync(string sql);
		Task<IRepositoryTransactionContext> BeginTransactionAsync();
		Task<IRepositoryTransactionContext> BeginTransactionAsync(ContextIsolationLevel isolationLevel);
		Task<bool> UseTransactionAsync(IRepositoryTransactionContext transactionContext);
		Task<int> SaveAsync();
	}
}

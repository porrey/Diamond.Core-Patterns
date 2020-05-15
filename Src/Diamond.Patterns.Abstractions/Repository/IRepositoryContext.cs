// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Defines the  isolation level to be used in transactions.
	/// </summary>
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
		/// <summary>
		/// Indicates to the data store that it should prepare for a large volume of
		/// new records to be added. This may not be supported on all data stores.
		/// </summary>
		/// <returns></returns>
		Task EnableBulkLoadAsync();

		/// <summary>
		/// Indicates to the data store that the bulk operation has completed or has
		/// been canceled. This may not be supported on all data stores.
		/// </summary>
		/// <returns></returns>
		Task DisableBulkLoadAsync();

		/// <summary>
		/// Executes a SQL command against the data store. This may not be supported
		/// on all data stores.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		Task<int> ExecuteSqlCommandAsync(string sql);

		/// <summary>
		/// Starts a transaction on the data store. This may not be supported on all data stores.
		/// </summary>
		/// <returns>Returns a transaction context that can  be used to manage the transaction.</returns>
		Task<IRepositoryTransactionContext> BeginTransactionAsync();

		/// <summary>
		/// Starts a transaction on the data store. This may not be supported on all data stores.
		/// </summary>
		/// <param name="isolationLevel">Indicates the isolation level to use for the new transaction.</param>
		/// <returns>Returns a transaction context that can  be used to manage the transaction.</returns>
		Task<IRepositoryTransactionContext> BeginTransactionAsync(ContextIsolationLevel isolationLevel);

		/// <summary>
		/// Uses an existing transaction on the data store. This may not be supported on all data stores.
		/// </summary>
		/// <param name="transactionContext">The current  transaction context to be used.</param>
		/// <returns>returns true if successful; false otherwise.</returns>
		Task<bool> UseTransactionAsync(IRepositoryTransactionContext transactionContext);

		/// <summary>
		/// Saves all changes in  the current instance.
		/// </summary>
		/// <returns>Returns the number of items affected by the save operation.</returns>
		Task<int> SaveAsync();
	}
}

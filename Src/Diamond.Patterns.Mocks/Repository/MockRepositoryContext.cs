using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Mocks
{
	public class MockRepositoryContext : IRepositoryContext
	{
		public Task<IRepositoryTransactionContext> BeginTransactionAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IRepositoryTransactionContext> BeginTransactionAsync(ContextIsolationLevel isolationLevel)
		{
			throw new NotImplementedException();
		}

		public Task DisableBulkLoadAsync()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
		}

		public Task EnableBulkLoadAsync()
		{
			throw new NotImplementedException();
		}

		public Task<int> ExecuteSqlCommandAsync(string sql)
		{
			throw new NotImplementedException();
		}

		public Task<int> SaveAsync()
		{
			throw new NotImplementedException();
		}

		public Task<bool> UseTransactionAsync(IRepositoryTransactionContext transactionContext)
		{
			throw new NotImplementedException();
		}
	}
}

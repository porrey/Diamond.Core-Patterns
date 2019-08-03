using System.Data.Entity;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Repository.EntityFramework
{
	public abstract class RepositoryContext<TContext> : DbContext, IRepositoryContext
	{
		public RepositoryContext(IStorageConfiguration storageConfiguration, IDatabaseStrategy<TContext> databaseStrategy)
			: base(storageConfiguration.ConnectionString)
		{
			this.DatabaseStrategy = databaseStrategy;
		}

		protected IDatabaseStrategy<TContext> DatabaseStrategy { get; set; }

		public Task<IRepositoryTransactionContext> BeginTransactionAsync()
		{
			return Task.FromResult<IRepositoryTransactionContext>(new RepositoryTransactionContext(this.Database.BeginTransaction()));
		}

		public Task<int> SaveAsync()
		{
			return this.SaveChangesAsync();
		}

		public Task<bool> UseTransactionAsync(IRepositoryTransactionContext transactionContext)
		{
			bool returnValue = false;

			if (transactionContext is RepositoryTransactionContext rtc)
			{
				this.Database.UseTransaction(rtc.Context.UnderlyingTransaction);
				returnValue = true;
			}

			return Task.FromResult(returnValue);
		}
	}
}
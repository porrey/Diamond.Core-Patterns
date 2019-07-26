using System.Data.Entity;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Diamond.Patterns.Repository.EntityFramework;

namespace Lsc.Logistics.TechBytes.AsyncAwait.SampleData
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
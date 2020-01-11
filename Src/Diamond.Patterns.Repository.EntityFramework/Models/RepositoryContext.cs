using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Repository.EntityFramework
{
	public abstract class RepositoryContext<TContext> : DbContext, IRepositoryContext
		where TContext : DbContext
	{
		public RepositoryContext(IStorageConfiguration storageConfiguration, IDatabaseStrategy<TContext> databaseStrategy)
			: base(storageConfiguration.ConnectionString)
		{
			this.DatabaseStrategy = databaseStrategy;
		}

		public RepositoryContext(IStorageConfiguration storageConfiguration)
			: base(storageConfiguration.ConnectionString)
		{
			this.DatabaseStrategy = null;
		}

		protected virtual IDatabaseStrategy<TContext> DatabaseStrategy { get; set; }

		public virtual Task<IRepositoryTransactionContext> BeginTransactionAsync(ContextIsolationLevel isolationLevel)
		{
			IsolationLevel localIsolationLevel = RepositoryContext<TContext>.TranslatioIsolationLevel(isolationLevel);
			return Task.FromResult<IRepositoryTransactionContext>(new RepositoryTransactionContext(this.Database.BeginTransaction(localIsolationLevel)));
		}

		public virtual Task<IRepositoryTransactionContext> BeginTransactionAsync()
		{
			return Task.FromResult<IRepositoryTransactionContext>(new RepositoryTransactionContext(this.Database.BeginTransaction()));
		}

		public virtual Task<int> SaveAsync()
		{
			return this.SaveChangesAsync();
		}

		public virtual Task<bool> UseTransactionAsync(IRepositoryTransactionContext transactionContext)
		{
			bool returnValue = false;

			if (transactionContext is RepositoryTransactionContext rtc)
			{
				this.Database.UseTransaction(rtc.Context.UnderlyingTransaction);
				returnValue = true;
			}

			return Task.FromResult(returnValue);
		}

		public static IsolationLevel TranslatioIsolationLevel(ContextIsolationLevel isolationLevel)
		{
			IsolationLevel returnValue = IsolationLevel.Chaos;

			switch (isolationLevel)
			{
				case ContextIsolationLevel.ReadStability:
					returnValue = IsolationLevel.ReadCommitted;
					break;
				case ContextIsolationLevel.RepeatableRead:
					returnValue = IsolationLevel.RepeatableRead;
					break;
				case ContextIsolationLevel.UncommitedRead:
					returnValue = IsolationLevel.ReadUncommitted;
					break;
				case ContextIsolationLevel.CursorStability:
					returnValue = IsolationLevel.Snapshot;
					break;
			}

			return returnValue;
		}

		public virtual Task EnableBulkLoadAsync()
		{
			this.Configuration.AutoDetectChangesEnabled = false;
			this.Configuration.ValidateOnSaveEnabled = false;
			return Task.FromResult(0);
		}

		public  Task DisableBulkLoadAsync()
		{
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ValidateOnSaveEnabled = true;
			return Task.FromResult(0);
		}

		public virtual Task<int> ExecuteSqlCommandAsync(string sql)
		{
			return this.Database.ExecuteSqlCommandAsync(sql);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			if (this.DatabaseStrategy != null)
			{
				IDatabaseInitializer<TContext> initializer = this.DatabaseStrategy.GetInitializer(modelBuilder, this.OnSeedDatabase) as IDatabaseInitializer<TContext>;
				Database.SetInitializer(initializer);
			}
		}

		protected virtual void OnSeedDatabase(object sender, TContext e)
		{
		}
	}
}
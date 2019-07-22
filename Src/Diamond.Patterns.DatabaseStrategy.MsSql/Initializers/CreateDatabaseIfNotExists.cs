using System;
using System.Data.Entity;

namespace Diamond.Patterns.DatabaseStrategy.MsSql
{
	internal class MsSqlCreateDatabaseIfNotExists<TContext> : CreateDatabaseIfNotExists<TContext> where TContext : DbContext
	{
		protected EventHandler<TContext> SeedCallback = null;

		public MsSqlCreateDatabaseIfNotExists(EventHandler<TContext> seedCallback)
			: base()
		{
			this.SeedCallback = seedCallback;
		}

		protected override void Seed(TContext context)
		{
			this.SeedCallback?.Invoke(this, context);
		}
	}
}

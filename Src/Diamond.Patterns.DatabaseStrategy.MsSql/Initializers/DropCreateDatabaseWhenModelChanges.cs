using System;
using System.Data.Entity;

namespace Diamond.Patterns.DatabaseStrategy.MsSql
{
	internal class MsSqlDropCreateDatabaseIfModelChanges<TContext> : DropCreateDatabaseIfModelChanges<TContext> where TContext : DbContext
	{
		protected EventHandler<TContext> SeedCallback = null;

		public MsSqlDropCreateDatabaseIfModelChanges(EventHandler<TContext> seedCallback)
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

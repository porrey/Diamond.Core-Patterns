using System;
using System.Data.Entity;
using SQLite.CodeFirst;

namespace Diamond.Patterns.DatabaseStrategy.SQLite
{
	internal class DropCreateDatabaseAlways<TContext> : SqliteDropCreateDatabaseAlways<TContext> where TContext : DbContext
	{
		protected EventHandler<TContext> SeedCallback = null;

		public DropCreateDatabaseAlways(DbModelBuilder modelBuilder, EventHandler<TContext> seedCallback)
			: base(modelBuilder)
		{
			this.SeedCallback = seedCallback;
		}

		protected override void Seed(TContext context)
		{
			this.SeedCallback?.Invoke(this, context);
		}
	}
}

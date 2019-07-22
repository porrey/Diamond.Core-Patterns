using System;
using System.Data.Entity;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.DatabaseStrategy.SQLite
{
	public class SQLiteNullStrategy<TContext> : IDatabaseStrategy<TContext> where TContext : DbContext
	{
		public object GetInitializer(object modelBuilder, EventHandler<TContext> onSeed)
		{
			return new NullDatabaseInitializer<TContext>();
		}
	}
}

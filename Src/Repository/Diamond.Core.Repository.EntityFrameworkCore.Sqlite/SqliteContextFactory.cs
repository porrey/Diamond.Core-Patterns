using Diamond.Patterns.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Repository.EntityFrameworkCore.Sqlite
{
	public class SqliteContextFactory<TContext> : ContextFactory<TContext>
		where TContext : IRepositoryContext, ISupportsConfiguration, new()
	{
		public SqliteContextFactory()
			: base()
		{
		}

		public SqliteContextFactory(string connectionString)
			: base(connectionString)
		{
			this.ConnectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite(this.ConnectionString);
		}
	}
}

using Diamond.Core.Repository.EntityFrameworkCore.Sqlite;
using Diamond.Patterns.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Repository.EntityFrameworkCore.SqlServer
{
	public class SqlServerContextFactory<TContext> : ContextFactory<TContext>
		where TContext : IRepositoryContext, ISupportsConfiguration, new()
	{
		public SqlServerContextFactory()
			: base()
		{
		}

		public SqlServerContextFactory(string connectionString)
			: base(connectionString)
		{
			this.ConnectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(this.ConnectionString);
		}
	}
}

using Diamond.Core.Repository.EntityFrameworkCore.Sqlite;
using Diamond.Patterns.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Repository.EntityFrameworkCore.InMemory
{
	public class InMemoryContextFactory<TContext> : ContextFactory<TContext>
		where TContext : IRepositoryContext, ISupportsConfiguration, new()
	{
		public InMemoryContextFactory()
			: base()
		{
		}

		public InMemoryContextFactory(string connectionString)
			: base(connectionString)
		{
			this.ConnectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseInMemoryDatabase("InMemory");
		}
	}
}

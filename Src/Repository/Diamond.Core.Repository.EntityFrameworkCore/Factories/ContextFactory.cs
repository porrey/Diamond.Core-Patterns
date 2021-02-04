using System.Threading.Tasks;
using Diamond.Patterns.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Repository.EntityFrameworkCore.Sqlite
{
	public class ContextFactory<TContext> : IContextFactory<TContext>
		where TContext : IRepositoryContext, ISupportsConfiguration, new()
	{
		public ContextFactory()
		{
		}

		public ContextFactory(string connectionString)
		{
			this.ConnectionString = connectionString;
		}

		public virtual TContext CreateContext()
		{
			TContext returnValue = default;

			returnValue = new TContext
			{
				ConfiguringCallback = this.OnConfiguring
			};

			return returnValue;
		}

		protected virtual string ConnectionString { get; set; }

		public virtual Task<TContext> CreateContextAsync()
		{
			return Task.FromResult(this.CreateContext());
		}

		protected virtual void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}
	}
}

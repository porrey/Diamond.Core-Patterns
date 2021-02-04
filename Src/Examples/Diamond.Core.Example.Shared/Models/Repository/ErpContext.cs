using Diamond.Core.Repository.EntityFrameworkCore;
using Diamond.Patterns.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Example
{
	public class ErpContext : RepositoryContext<ErpContext>, ISupportsConfiguration
	{
		public ErpContext()
			: base()
		{
		}

		public ErpContext(DbContextOptions options)
			: base(options)
		{
		}

		public ErpContext(OnConfiguringDelegate configuringCallback)
			: base()
		{
			this.ConfiguringCallback = configuringCallback;
		}

		public DbSet<Invoice> Invoices { get; set; }

		public OnConfiguringDelegate ConfiguringCallback { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			this.ConfiguringCallback?.Invoke(optionsBuilder);
		}
	}
}

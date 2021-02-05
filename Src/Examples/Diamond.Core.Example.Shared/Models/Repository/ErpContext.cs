using Diamond.Core.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Example
{
	public class ErpContext : RepositoryContext<ErpContext>
	{
		public ErpContext()
			: base()
		{
		}

		public ErpContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Invoice> Invoices { get; set; }
	}
}

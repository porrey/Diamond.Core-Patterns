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

		public DbSet<InvoiceEntity> Invoices { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// ***
			// *** Invoice number must be unique.
			// ***
			modelBuilder.Entity<InvoiceEntity>().HasIndex(p => p.Number).IsUnique();

			base.OnModelCreating(modelBuilder);
		}
	}
}

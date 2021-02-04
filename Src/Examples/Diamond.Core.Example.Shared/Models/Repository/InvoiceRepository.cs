using Diamond.Core.Repository;
using Diamond.Core.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Example
{
	public  class InvoiceRepository : EntityFrameworkRepository<IInvoice, Invoice, ErpContext>
	{
		public InvoiceRepository(IContextFactory<ErpContext> contextFactory, IEntityFactory<IInvoice> modelFactory)
			: base(contextFactory, modelFactory)
		{
		}

		protected override DbSet<Invoice> MyDbSet(ErpContext context) => context.Invoices;
		protected override ErpContext GetNewDbContext => this.ContextFactory.CreateContext();
	}
}
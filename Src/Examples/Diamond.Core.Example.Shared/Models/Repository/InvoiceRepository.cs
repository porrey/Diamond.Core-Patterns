using Diamond.Core.Repository;
using Diamond.Core.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Example
{
	public  class InvoiceRepository : EntityFrameworkRepository<IInvoice, Invoice, ErpContext>
	{
		public InvoiceRepository(ErpContext context, IEntityFactory<IInvoice> modelFactory)
			: base(context, modelFactory)
		{
		}

		protected override DbSet<Invoice> MyDbSet(ErpContext context) => context.Invoices;
	}
}
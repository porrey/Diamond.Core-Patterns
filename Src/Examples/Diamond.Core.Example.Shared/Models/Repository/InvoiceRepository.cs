using Diamond.Core.Repository;
using Diamond.Core.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Example {
	public class InvoiceRepository : EntityFrameworkRepository<IInvoice, InvoiceEntity, ErpContext> {
		public InvoiceRepository(ErpContext context, IEntityFactory<IInvoice> modelFactory)
			: base(context, modelFactory) {
		}

		protected override DbSet<InvoiceEntity> MyDbSet(ErpContext context) => context.Invoices;
	}
}
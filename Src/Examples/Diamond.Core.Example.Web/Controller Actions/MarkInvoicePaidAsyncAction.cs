using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.AspNet.DoAction;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example {
	/// <summary>
	/// 
	/// </summary>
	public class MarkInvoicePaidAsyncAction : IDoAction<(string InvoiceNumber, bool Paid), IControllerActionResult<Invoice>> {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public MarkInvoicePaidAsyncAction(ILogger<MarkInvoicePaidAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper) {
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
			this.Mapper = mapper;
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<MarkInvoicePaidAsyncAction> Logger { get; set; }

		/// <summary>
		/// Holds the reference to <see cref="IRepositoryFactory"/>.
		/// </summary>
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected IMapper Mapper { get; set; }

		/// <summary>
		/// As a best practice, the name of this class should match the controller
		/// method name with the word "Action" appended to the end. The DoActionController
		/// uses [CallerMemberName] as the action key by default.
		/// </summary>
		public string ActionKey => typeof(MarkInvoicePaidAsyncAction).Name.Replace("Action", "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<IControllerActionResult<Invoice>> ExecuteActionAsync((string InvoiceNumber, bool Paid) item) {
			ControllerActionResult<Invoice> returnValue = new ControllerActionResult<Invoice>();

			//
			// Get a writable repository for IInvoice.
			//
			this.Logger.LogTrace("Retrieving a writable repository for IInvoice.");
			IWritableRepository<IInvoice> repository = await this.RepositoryFactory.GetWritableAsync<IInvoice>();

			//
			// Get the invoice.
			//
			IInvoice exisingItem = (await repository.GetAsync(t => t.Number == item.InvoiceNumber)).SingleOrDefault();

			if (exisingItem != null) {
				//
				// Update the existing item.
				//
				exisingItem.Paid = item.Paid;

				//
				// Update the data.
				//
				bool result = await repository.UpdateAsync(exisingItem);

				if (result) {
					returnValue.ResultDetails = DoActionResult.Ok();
					returnValue.Result = this.Mapper.Map<Invoice>(exisingItem);
				}
				else {
					returnValue.ResultDetails = DoActionResult.BadRequest($"The invoice with invoice number '{item.InvoiceNumber}' could not be updated.");
				}
			}
			else {
				returnValue.ResultDetails = DoActionResult.NotFound($"An invoice with invoice number '{item.InvoiceNumber}' could not be found.");
			}

			return returnValue;
		}
	}
}

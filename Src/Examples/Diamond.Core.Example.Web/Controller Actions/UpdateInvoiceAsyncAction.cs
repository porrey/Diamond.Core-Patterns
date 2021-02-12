using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.AspNet.DoAction;
using Diamond.Core.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example {
	/// <summary>
	/// 
	/// </summary>
	public class UpdateInvoiceAsyncAction : DoAction<(string InvoiceNumber, JsonPatchDocument<Invoice> Invoice), Invoice> {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public UpdateInvoiceAsyncAction(ILogger<UpdateInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
			: base(logger) {
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
			this.Mapper = mapper;
		}

		/// <summary>
		/// Holds the reference to <see cref="IRepositoryFactory"/>.
		/// </summary>
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected IMapper Mapper { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public override Task<(bool, string)> ValidateModel((string InvoiceNumber, JsonPatchDocument<Invoice> Invoice) item) {
			(bool result, string errorString) = (true, null);

			//
			// Get the model from the patch.
			//
			Invoice patched = new Invoice();
			item.Invoice.ApplyTo(patched);

			//
			// Check the invoice number.
			//
			if (patched.Number != null) {
				result = false;
				errorString = "Invoice Number cannot be updated.";
			}

			return Task.FromResult((result, errorString));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override async Task<IControllerActionResult<Invoice>> OnExecuteActionAsync((string InvoiceNumber, JsonPatchDocument<Invoice> Invoice) item) {
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
				// Apply the patch 
				//
				Invoice patched = new Invoice();
				this.Mapper.Map(exisingItem, patched);
				item.Invoice.ApplyTo(patched);

				//
				// Update the existing item.
				//
				this.Mapper.Map(patched, exisingItem);

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

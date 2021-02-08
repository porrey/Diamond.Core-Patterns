using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Diamond.Core.AspNet.DoAction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.Core.Example
{
	/// <summary>
	/// Retrieves invoice informtion from the ERP system.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	[ApiVersion("1.0")]
	[Produces("application/json", "application/xml")]
	[Description("Retrieves invoice informtion from the ERP system.")]
	public class ErpInvoiceController : DoActionController
	{
		/// <summary>
		/// Creates an instance of <see cref="ErpInvoiceController"/> with a dependency
		/// on <see cref="IDoActionFactory"/>.
		/// </summary>
		/// <param name="doActionFactory"></param>
		public ErpInvoiceController(IDoActionFactory doActionFactory)
			: base(doActionFactory)
		{
		}

		/// <summary>
		/// Gets a specific invoice by invoice number.
		/// </summary>
		/// <param name="invoiceNumber">The uniue number for a given invoice.</param>
		/// <returns>Gets an invoice from the ERP system matching the given invoice number.</returns>
		/// <response code="200">The invoice was succssully retrieved from the ERP system with the given invoice number.</response>
		/// <response code="400">The request failed due to a client error. In some cases, the response will have more details.</response>
		/// <response code="404">An invoice with the specified invoice number was not found.</response>
		[HttpGet("{invoiceNumber}")]
		[ProducesResponseType(typeof(InvoiceItem), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(FailedRequest), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(FailedRequest), StatusCodes.Status404NotFound)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<InvoiceItem>> GetInvoiceAsync(string invoiceNumber)
		{
			this.LogMethodCall();
			return this.Do<string, InvoiceItem>(invoiceNumber);
		}

		/// <summary>
		/// Gets the full list of invoices available.
		/// </summary>
		/// <returns>A list of allavailable invoices in the ERP system.</returns>
		/// <response code="200">A list of allavailable invoices in the ERP system was successfully retrieved.</response>
		/// <response code="404">There are currently no invoices available in the system.</response>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<InvoiceItem>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(FailedRequest), StatusCodes.Status404NotFound)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<IEnumerable<InvoiceItem>>> GetAllInvoicesAsync()
		{
			this.LogMethodCall();
			return this.Do<IEnumerable<InvoiceItem>>();
		}

		/// <summary>
		/// Create a new invoice item.
		/// </summary>
		/// <param name="item">The details of the new invoice.</param>
		/// <response code="200">The invoice item was successfully created.</response>
		/// <response code="400">The invoice details were invalid or resulted in a duplicate invoice.</response>
		/// <returns>The newly create invoice.</returns>
		[HttpPost]
		[ProducesResponseType(typeof(IEnumerable<InvoiceItem>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(FailedRequest), StatusCodes.Status404NotFound)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<InvoiceItem>> CreateInvoiceAsync([FromBody]InvoiceItem item)
		{
			this.LogMethodCall();
			return this.Do<InvoiceItem, InvoiceItem>(item);
		}

		/// <summary>
		/// Update and existing invoice item.
		/// </summary>
		/// <param name="item">The details of the new invoice.</param>
		/// <response code="200">The invoice item was successfully created.</response>
		/// <response code="400">The invoice could not be updated.</response>
		/// <response code="404">The invoice specified was not found.</response>
		/// <returns>The updated create invoice.</returns>
		[HttpPut]
		[ProducesResponseType(typeof(IEnumerable<InvoiceItem>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(FailedRequest), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(FailedRequest), StatusCodes.Status400BadRequest)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<InvoiceItem>> UpdateInvoiceAsync([FromBody] InvoiceItem item)
		{
			this.LogMethodCall();
			return this.Do<InvoiceItem, InvoiceItem>(item);
		}
	}
}

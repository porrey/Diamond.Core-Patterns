//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Diamond.Core.AspNet.DoAction;
using Diamond.Core.AspNet.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Diamond.Core.Example
{
	/// <summary>
	/// Retrieves invoice information from the ERP system.
	/// </summary>y
	[ApiController]
	[Route("api/[controller]")]
	[ApiVersion("1.0")]
	[Produces("application/json", "application/xml")]
	[Description("Retrieves invoice information from the ERP system.")]
	public class InvoicesController : DoActionController
	{
		/// <summary>
		/// Creates an instance of <see cref="InvoicesController"/> with a dependency
		/// on <see cref="IDoActionFactory"/>.
		/// </summary>
		/// <param name="doActionFactory"></param>
		public InvoicesController(IDoActionFactory doActionFactory)
			: base(doActionFactory)
		{
		}

		/// <summary>
		/// Gets a specific invoice by invoice number.
		/// </summary>
		/// <param name="invoiceNumber">The unique invoice number of the invoice to retrieve.</param>
		/// <returns>Gets an invoice from the ERP system matching the given invoice number.</returns>
		/// <response code="200">The invoice was successfully retrieved from the ERP system with the given invoice number.</response>
		/// <response code="400">The request failed due to a client error. In some cases, the response will have more details.</response>
		/// <response code="404">An invoice with the specified invoice number was not found.</response>
		[HttpGet("{invoiceNumber}")]
		[ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<Invoice>> GetInvoiceAsync(string invoiceNumber)
		{
			this.LogMethodCall();
			return this.Do<string, Invoice>(invoiceNumber);
		}

		/// <summary>
		/// Gets the full list of invoices available.
		/// </summary>
		/// <returns>A list of all available invoices in the ERP system.</returns>
		/// <response code="200">A list of all available invoices in the ERP system was successfully retrieved.</response>
		/// <response code="404">There are currently no invoices available in the system.</response>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Invoice>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<IEnumerable<Invoice>>> GetAllInvoicesAsync()
		{
			this.LogMethodCall();
			return this.Do<IEnumerable<Invoice>>();
		}

		/// <summary>
		/// Create a new invoice.
		/// </summary>
		/// <param name="item">The details of the new invoice.</param>
		/// <response code="200">The invoice was successfully created.</response>
		/// <response code="400">The invoice details were invalid or resulted in a duplicate invoice.</response>
		/// <returns>The newly create invoice.</returns>
		[HttpPost]
		[ProducesResponseType(typeof(IEnumerable<Invoice>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<Invoice>> CreateInvoiceAsync([FromBody] Invoice item)
		{
			this.LogMethodCall();
			return this.Do<Invoice, Invoice>(item);
		}

		/// <summary>
		/// Mark an existing invoice paid/unpaid.
		/// </summary>
		/// <param name="invoiceNumber">The unique invoice number of the invoice to update.</param>
		/// <param name="paid">Determines whether to mark the invoice paid (true) or unpaid (false).</param>
		/// <response code="200">The invoice was successfully created.</response>
		/// <response code="400">The invoice could not be updated.</response>
		/// <response code="404">The invoice specified was not found.</response>
		/// <returns>The details of the updated invoice.</returns>
		[HttpPut("{invoiceNumber}/{paid}")]
		[ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<Invoice>> MarkInvoicePaidAsync(string invoiceNumber, bool paid)
		{
			this.LogMethodCall();
			return this.Do<(string, bool), Invoice>((invoiceNumber, paid));
		}

		/// <summary>
		/// Updates an invoice using the JSON patch document.
		/// </summary>
		/// <param name="invoiceNumber">The unique invoice number of the invoice to delete.</param>
		/// <param name="item">The updated details of the invoice.</param>
		/// <returns></returns>
		[HttpPatch("{invoiceNumber}")]
		[ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		[Consumes("application/json", "application/xml")]
		[SwaggerRequestExample(typeof(Operation), typeof(JsonPatchDefaultExample))]
		public Task<ActionResult<Invoice>> UpdateInvoiceAsync(string invoiceNumber, [FromBody] JsonPatchDocument<Invoice> item)
		{
			this.LogMethodCall();
			return this.Do<(string, JsonPatchDocument<Invoice>), Invoice>((invoiceNumber, item));
		}

		/// <summary>
		/// Delete an existing invoice.
		/// </summary>
		/// <param name="invoiceNumber">The unique invoice number of the invoice to delete.</param>
		/// <response code="200">The invoice was successfully deleted.</response>
		/// <response code="404">The invoice specified was not found.</response>
		/// <returns>The details of the invoice that was deleted.</returns>
		[HttpDelete("{invoiceNumber}")]
		[ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<Invoice>> DeleteInvoiceAsync(string invoiceNumber)
		{
			this.LogMethodCall();
			return this.Do<string, Invoice>(invoiceNumber);
		}

		/// <summary>
		/// Use this method to change the details of the problem details
		/// returned to th client when the return status is anything other
		/// than 200.
		/// </summary>
		/// <param name="problemDetails">The instance of <see cref="ProblemDetails"/> that will be returned to the client.</param>
		/// <returns>An instance of <see cref="ProblemDetails"/>.</returns>
		protected override ProblemDetails OnCreateProblemDetail(ProblemDetails problemDetails)
		{
			return base.OnCreateProblemDetail(problemDetails);
		}
	}
}

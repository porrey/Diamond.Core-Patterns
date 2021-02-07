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
	public class ErpInvoiceController : DoActionControllerBase
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
		/// <returns>An instance of <see cref="Invoice"/> match the given invoice number.</returns>
		[HttpGet("{invoiceNumber}")]
		[ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(FailedRequest), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(FailedRequest), StatusCodes.Status404NotFound)]
		[Consumes("application/json", "application/xml")]
		public Task<ActionResult<Invoice>> GetInvoice(string invoiceNumber)
		{
			this.LogMethodCall();
			return this.Do<string, Invoice>(nameof(GetInvoice), invoiceNumber);
		}
	}
}

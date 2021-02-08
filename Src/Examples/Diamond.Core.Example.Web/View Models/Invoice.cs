using System.ComponentModel.DataAnnotations;

namespace Diamond.Core.Example
{
	/// <summary>
	/// Contains the details of an invoice.
	/// </summary>
	public class Invoice : InvoiceUpdate
	{
		/// <summary>
		/// The unique invoice number.
		/// </summary>
		[Required]
		[MaxLength(30)]
		[RegularExpression("INV[a-zA-Z0-9]*")]
		[Display(Order = 0, Prompt = "Invoice Number")]
		public string Number { get; set; }
	}
}

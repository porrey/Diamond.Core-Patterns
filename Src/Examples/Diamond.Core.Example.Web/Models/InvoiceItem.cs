using System.ComponentModel.DataAnnotations;

namespace Diamond.Core.Example
{
	/// <summary>
	/// The details for a given invoice.
	/// </summary>
	public class InvoiceItem
	{
		/// <summary>
		/// The unique data storage ID for this invoice.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The unique invoice number.
		/// </summary>
		[Required]
		[MaxLength(30)]
		[RegularExpression("INV[a-zA-Z0-9]*", ErrorMessage = "")]
		public string Number { get; set; }

		/// <summary>
		/// A description of invoice.
		/// </summary>
		[Required]
		[MaxLength(100)]
		public string Description { get; set; }

		/// <summary>
		/// The total dollar amoint of the invoice.
		/// </summary>
		[Required]
		public float Total { get; set; }
	}
}

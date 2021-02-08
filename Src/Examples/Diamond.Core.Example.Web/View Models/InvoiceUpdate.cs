using System.ComponentModel.DataAnnotations;

namespace Diamond.Core.Example
{
	/// <summary>
	/// Contains the details of an invoice that support being updated.
	/// </summary>
	public class InvoiceUpdate
	{
		/// <summary>
		/// A description of invoice.
		/// </summary>
		[Required]
		[MaxLength(100)]
		[Display(Order = 1, Prompt = "Invoice Description")]
		public string Description { get; set; }

		/// <summary>
		/// The total dollar amount of the invoice.
		/// </summary>
		[Required]
		[Range(0, float.MaxValue)]
		[Display(Order = 2, Prompt = "Invoice Total")]
		public float Total { get; set; }
	}
}

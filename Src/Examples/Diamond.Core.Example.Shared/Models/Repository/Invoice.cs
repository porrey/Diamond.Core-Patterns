using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Diamond.Core.Repository;

namespace Diamond.Core.Example
{
	[Table("Invoice", Schema = "Financial")]
	public class Invoice : Entity<int>, IInvoice
	{
		[Column("InvoiceId", Order = 0)]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required]
		public new int Id { get; set; }

		[Required]
		public string Number { get; set; }

		[Required]
		public float Total { get; set; }

		[Required]
		public string Description { get; set; }
	}
}

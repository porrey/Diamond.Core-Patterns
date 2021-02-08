using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Diamond.Core.Repository;

namespace Diamond.Core.Example
{
	[Table("Invoice", Schema = "Financial")]
	public class InvoiceEntity : Entity<int>, IInvoice
	{
		[Column("InvoiceId", Order = 0)]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required]
		public new int Id { get; set; }

		[Required]
		[MaxLength(30)]
		public string Number { get; set; }

		[Required]
		public float Total { get; set; }

		[Required]
		[MaxLength(100)]
		public string Description { get; set; }

		public bool Paid { get; set; }
	}
}


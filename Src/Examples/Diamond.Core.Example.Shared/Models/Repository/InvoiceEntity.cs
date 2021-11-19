//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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


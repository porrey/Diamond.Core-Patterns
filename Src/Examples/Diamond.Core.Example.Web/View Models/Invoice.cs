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
using System.ComponentModel.DataAnnotations;

namespace Diamond.Core.Example
{
	/// <summary>
	/// Contains the details of an invoice.
	/// </summary>
	public class Invoice
	{
		/// <summary>
		/// The unique invoice number.
		/// </summary>
		[Required]
		[MaxLength(30)]
		[RegularExpression("INV[a-zA-Z0-9]*")]
		[Display(Order = 0, Prompt = "Invoice Number")]
		public string Number { get; set; }

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

		/// <summary>
		/// Determines if the invoice has been paid or not.
		/// </summary>
		[Display(Order = 3, Prompt = "Paid")]
		public bool Paid { get; set; }
	}
}

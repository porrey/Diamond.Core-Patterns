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
using System.Text.Json.Serialization;

namespace Diamond.Core.Example
{
	/// <summary>
	/// Contains the details of an invoice.
	/// </summary>
	public class Invoice : InvoiceNumber
	{
		/// <summary>
		/// A description of invoice.
		/// </summary>
		[Required]
		[MaxLength(100)]
		[Display(Order = 1, Name = "description", ShortName = "d", Description = "A description of invoice.")]
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>
		/// The total dollar amount of the invoice.
		/// </summary>
		[Required]
		[Range(0, float.MaxValue)]
		[Display(Order = 2, Name = "total", ShortName = "t", Description = "The total dollar amount of the invoice.")]
		[JsonPropertyName("total")]
		public float Total { get; set; }

		/// <summary>
		/// Indicates if the invoice has been paid or not.
		/// </summary>
		[Display(Order = 3, Name = "paid", ShortName = "p", Description = "Indicates if the invoice has been paid or not.")]
		[JsonPropertyName("paid")]
		public bool Paid { get; set; }

		public override string ToString()
		{
			string status = this.Paid ? "Paid" : "Not Paid";
			return $"{this.Number}: '{this.Description}', {this.Total:$#,##0.00} [{status}]";
		}
	}
}

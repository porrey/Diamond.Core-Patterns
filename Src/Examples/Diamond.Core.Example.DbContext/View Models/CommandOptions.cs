//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
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
	/// Database actions.
	/// </summary>
	public enum Action
	{
		/// <summary>
		/// Create the database.
		/// </summary>
		Create,
		/// <summary>
		/// Drop the database.
		/// </summary>
		Drop
	}

	/// <summary>
	/// Contains the details of an invoice.
	/// </summary>
	public class CommandOptions
	{
		/// <summary>
		/// A description of invoice.
		/// </summary>
		[Required]
		[Display(Order = 1, Name = "action", ShortName = "a", Description = "Specifies the action on the database.")]
		[JsonPropertyName("action")]
		public Action Action { get; set; }
	}
}

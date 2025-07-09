//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
using Microsoft.AspNetCore.Http;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Defines the contract for a DataTable request, including pagination, sorting, and search criteria.
	/// </summary>
	public interface IDataTableRequest : IDataTablesObject
	{
		/// <summary>
		/// Gets or sets the collection of columns associated with the request.
		/// </summary>
		Column[] Columns { get; set; }

		/// <summary>
		/// Gets or sets the draw count for the current operation.
		/// </summary>
		int Draw { get; set; }

		/// <summary>
		/// Gets or sets the length of the items to return.
		/// </summary>
		int Length { get; set; }

		/// <summary>
		/// Gets or sets the collection of order instructions for sorting.
		/// </summary>
		Order[] Order { get; set; }

		/// <summary>
		/// Gets or sets the search configuration for the request.
		/// </summary>
		Search Search { get; set; }

		/// <summary>
		/// Gets or sets the search builder configuration for the form.
		/// </summary>
		FormCollection SearchBuilder { get; set; }

		/// <summary>
		/// Gets or sets the starting index for the operation.
		/// </summary>
		int Start { get; set; }
	}
}
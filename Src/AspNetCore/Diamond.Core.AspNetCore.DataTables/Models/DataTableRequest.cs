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
using Newtonsoft.Json;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Represents a request for data in a DataTable format, including pagination, sorting, and search criteria.
	/// </summary>
	/// <remarks>This class is used to encapsulate the parameters sent from a client-side DataTable to a server-side
	/// processing endpoint. It includes properties for pagination (such as <see cref="Start"/> and <see cref="Length"/>),
	/// sorting (via <see cref="Order"/>), and search criteria (via <see cref="Search"/> and <see
	/// cref="SearchBuilder"/>).</remarks>
	public class DataTableRequest : DataTablesObject, IDataTableRequest
	{
		/// <summary>
		/// Gets or sets the draw count for the current operation.
		/// </summary>
		[JsonProperty("draw")]
		public virtual int Draw { get; set; }

		/// <summary>
		/// Gets or sets the starting index for the operation.
		/// </summary>
		[JsonProperty("start")]
		public virtual int Start { get; set; }

		/// <summary>
		/// Gets or sets the length of the item.
		/// </summary>
		[JsonProperty("length")]
		public virtual int Length { get; set; }

		/// <summary>
		/// Gets or sets the search configuration for the current context.
		/// </summary>
		[JsonProperty("search")]
		public virtual Search Search { get; set; }

		/// <summary>
		/// Gets or sets the collection of orders associated with the current context.
		/// </summary>
		[JsonProperty("order")]
		public virtual Order[] Order { get; set; }

		/// <summary>
		/// Gets or sets the collection of columns associated with the entity.
		/// </summary>
		[JsonProperty("columns")]
		public virtual Column[] Columns { get; set; }

		/// <summary>
		/// Gets or sets the search builder configuration for the form.
		/// </summary>
		[JsonProperty("searchBuilder")]
		public virtual FormCollection SearchBuilder { get; set; }
	}
}

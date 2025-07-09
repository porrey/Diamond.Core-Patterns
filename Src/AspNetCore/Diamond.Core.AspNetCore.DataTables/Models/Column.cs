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
using Newtonsoft.Json;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Represents a column in a data table, including properties for title, data source, 
	/// searchability, orderability, and more.
	/// </summary>
	public class Column : DataTablesObject
	{
		/// <summary>
		/// Gets or sets the title associated with the object.
		/// </summary>
		[JsonProperty("title")]
		public virtual string Title { get; set; } = String.Empty;

		/// <summary>
		/// Gets or sets the data source for the column, which can be a property name or a field name.
		/// </summary>
		[JsonProperty("data")]
		public virtual string Data { get; set; } = String.Empty;

		/// <summary>
		/// Gets or sets the name of the column, which is used for identification purposes.
		/// </summary>
		[JsonProperty("name")]
		public virtual string Name { get; set; } = String.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether the column is searchable.
		/// </summary>
		[JsonProperty("searchable")]
		public virtual bool Searchable { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether the column is orderable.
		/// </summary>
		[JsonProperty("orderable")]
		public virtual bool Orderable { get; set; } = true;

		/// <summary>
		/// Gets or sets the search criteria for the column, which includes the search value and whether to use regular expressions.
		/// </summary>
		[JsonProperty("search")]
		public virtual Search Search { get; set; } = null;

		/// <summary>
		/// Gets or sets the type of data in the column, which can be used for formatting or validation purposes.
		/// </summary>
		[JsonProperty("type")]
		public virtual string Type { get; set; } = "string";

		/// <summary>
		/// Gets or sets a value indicating whether the column is filterable.
		/// </summary>
		[JsonProperty("filterable")]
		public virtual bool Filterable { get; set; } = true;
	}
}

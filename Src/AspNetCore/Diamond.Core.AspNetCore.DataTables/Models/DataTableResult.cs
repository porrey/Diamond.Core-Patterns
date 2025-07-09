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
	/// Represents the result of a data table operation, including the data items and metadata about the operation.
	/// </summary>
	/// <remarks>This class is typically used to encapsulate the results of a server-side data table query,
	/// including the data items and additional information such as the draw count and record totals. It is designed to be
	/// serialized to JSON for communication with client-side data table components.</remarks>
	/// <typeparam name="TItem">The type of the data items contained in the result.</typeparam>
	public class DataTableResult<TItem>
	{
		/// <summary>
		/// Gets or sets the array of data items returned by the data table operation.
		/// </summary>
		[JsonProperty("data")]
		public virtual TItem[] Data { get; set; }

		/// <summary>
		/// Gets or sets the draw count, which is used to ensure that the response corresponds 
		/// to the request made by the client.
		/// </summary>
		[JsonProperty("draw")]
		public virtual int Draw { get; set; }

		/// <summary>
		/// Gets or sets the number of records after filtering has been applied.
		/// </summary>
		[JsonProperty("recordsFiltered")]
		public virtual int RecordsFiltered { get; set; }

		/// <summary>
		/// Gets or sets the total number of records available in the data source, regardless of filtering.
		/// </summary>
		[JsonProperty("recordsTotal")]
		public virtual int RecordsTotal { get; set; }

		/// <summary>
		/// Gets or sets an error message, if any occurred during the data table operation.
		/// </summary>
		[JsonProperty("error")]
		public virtual string Error { get; set; }
	}
}

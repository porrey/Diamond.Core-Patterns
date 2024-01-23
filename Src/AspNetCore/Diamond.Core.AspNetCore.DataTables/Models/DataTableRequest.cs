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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Diamond.Core.AspNetCore.DataTables
{
	public class DataTableRequest : IDataTableRequest
	{
		[JsonProperty("draw")]
		public virtual int Draw { get; set; }

		[JsonProperty("start")]
		public virtual int Start { get; set; }

		[JsonProperty("length")]
		public virtual int Length { get; set; }

		[JsonProperty("search")]
		public virtual Search Search { get; set; }

		[JsonProperty("order")]
		public virtual Order[] Order { get; set; }

		[JsonProperty("columns")]
		public virtual Column[] Columns { get; set; }

		[JsonProperty("searchBuilder")]
		public virtual FormCollection SearchBuilder { get; set; }
	}
}

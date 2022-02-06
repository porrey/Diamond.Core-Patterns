using System;
using Newtonsoft.Json;

namespace Diamond.Core.AspNetCore.DataTables
{
	public class Column
	{
		[JsonProperty("title")]
		public string Title { get; set; } = String.Empty;

		[JsonProperty("data")]
		public string Data { get; set; } = String.Empty;

		[JsonProperty("name")]
		public string Name { get; set; } = String.Empty;

		[JsonProperty("searchable")]
		public bool Searchable { get; set; } = true;

		[JsonProperty("orderable")]
		public bool Orderable { get; set; } = true;

		[JsonProperty("search")]
		public Search Search { get; set; } = null;

		[JsonProperty("type")]
		public string Type { get; set; } = "string";

		[JsonProperty("filterable")]
		public bool Filterable { get; set; } = true;
	}
}

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Diamond.Core.AspNetCore.DataTables
{
	public class DataTableRequest : IDataTableRequest
	{
		[JsonProperty("draw")]
		public int Draw { get; set; }

		[JsonProperty("start")]
		public int Start { get; set; }

		[JsonProperty("length")]
		public int Length { get; set; }

		[JsonProperty("search")]
		public Search Search { get; set; }

		[JsonProperty("order")]
		public Order[] Order { get; set; }

		[JsonProperty("columns")]
		public Column[] Columns { get; set; }

		[JsonProperty("searchBuilder")]
		public FormCollection SearchBuilder { get; set; }
	}
}

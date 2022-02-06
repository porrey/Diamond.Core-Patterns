using Newtonsoft.Json;

namespace Diamond.Core.AspNetCore.DataTables
{
	public class DataTableResult<TItem>
	{
		[JsonProperty("data")]
		public TItem[] Data { get; set; }

		[JsonProperty("draw")]
		public int Draw { get; set; }

		[JsonProperty("recordsFiltered")]
		public int RecordsFiltered { get; set; }

		[JsonProperty("recordsTotal")]
		public int RecordsTotal { get; set; }

		[JsonProperty("error")]
		public string Error { get; set; }
	}
}

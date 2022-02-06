using Newtonsoft.Json;

namespace Diamond.Core.AspNetCore.DataTables
{
	public class Order
	{
		[JsonProperty("column")]
		public int Column { get; set; }

		[JsonProperty("dir")]
		public string Dir { get; set; }
	}
}

using System;
using Newtonsoft.Json;

namespace Diamond.Core.AspNetCore.DataTables
{
	public class Search
	{
		[JsonProperty("value")]
		public string Value { get; set; } = String.Empty;

		[JsonProperty("regex")]
		public bool RegEx { get; set; } = false;
	}
}

using Newtonsoft.Json;

namespace Diamond.Core.AspNet.Swagger {
	/// <summary>
	/// 
	/// </summary>
	public class Operation {
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("value")]
		public object Value { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("path")]
		public string Path { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("op")]
		public string Op { get; set; }
	}
}
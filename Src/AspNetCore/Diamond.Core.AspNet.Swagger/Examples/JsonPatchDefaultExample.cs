using Swashbuckle.AspNetCore.Filters;

namespace Diamond.Core.AspNet.Swagger {
	/// <summary>
	/// 
	/// </summary>
	public class JsonPatchDefaultExample : IExamplesProvider<Operation[]> {
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Operation[] GetExamples() {
			return this.OnGetExamples();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected Operation[] OnGetExamples() {
			return (Operation[])new[]
			{
				new Operation
				{
					Op = "replace",
					Path = "/name",
					Value = "John Doe"
				},
				new Operation
				{
					Op = "replace",
					Path = "/total",
					Value = "45.50"
				}
			};
		}
	}
}

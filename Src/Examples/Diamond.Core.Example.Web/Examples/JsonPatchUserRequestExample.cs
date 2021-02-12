using Swashbuckle.AspNetCore.Filters;

namespace Diamond.Core.Example {
	/// <summary>
	/// 
	/// </summary>
	public class JsonPatchUserRequestExample : IExamplesProvider<Operation[]>{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Operation[] GetExamples() {
			return new[]
			{
				new Operation
				{
					Op = "replace",
					Path = "/description",
						Value = "Computer programming invoice."
				},
				new Operation
				{
					Op = "replace",
					Path = "/Total",
						Value = "45.50"
				}
			};
		}
	}
}

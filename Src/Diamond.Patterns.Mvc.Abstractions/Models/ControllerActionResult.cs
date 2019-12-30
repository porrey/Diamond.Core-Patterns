using Diamond.Patterns.Abstractions;

#pragma warning disable CS1591

namespace Diamond.Patterns.Mvc.Abstractions
{
	public class ControllerActionResult<TResult> : IControllerActionResult<TResult>
	{
		public ResultType ResultType { get; set; }
		public string ErrorMessage { get; set; }
		public TResult Result { get; set; }
	}
}

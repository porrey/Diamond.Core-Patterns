#pragma warning disable CS1591

namespace Diamond.Patterns.Abstractions
{
	public enum ResultType
	{
		Ok,
		NotFound,
		BadRequest
	}

	public interface IControllerActionResult<TResult>
	{
		ResultType ResultType { get; set; }
		string ErrorMessage { get;  }
		TResult Result { get; }
	}
}

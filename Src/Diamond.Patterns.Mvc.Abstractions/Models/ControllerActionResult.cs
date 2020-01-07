using Diamond.Patterns.Abstractions;

#pragma warning disable CS1591

namespace Diamond.Patterns.Mvc.Abstractions
{
	/// <summary>
	/// Contains the result of a controller action.
	/// </summary>
	/// <typeparam name="TResult">The type of the inner object.</typeparam>
	public class ControllerActionResult<TResult> : IControllerActionResult<TResult>
	{
		/// <summary>
		/// The type of response usually associated to an HTTP status code.
		/// </summary>
		public ResultType ResultType { get; set; }

		/// <summary>
		/// A description of the error if the action failed.
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// The resulting object instance if the action was successful.
		/// </summary>
		public TResult Result { get; set; }
	}
}

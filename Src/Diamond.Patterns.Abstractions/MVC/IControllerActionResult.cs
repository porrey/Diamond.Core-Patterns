namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Specifies the result type of a controller action.
	/// </summary>
	public enum ResultType
	{
		/// <summary>
		/// The action result in success (usually a 200 status).
		/// </summary>
		Ok,
		/// <summary>
		/// The action result in not found (usually a 404 status).
		/// </summary>
		NotFound,
		/// <summary>
		/// The action result in bad request (usually a 400 status).
		/// </summary>
		BadRequest
	}

	/// <summary>
	/// Contains the result of a controller action.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IControllerActionResult<TResult>
	{
		/// <summary>
		/// The type of response usually associated to an HTTP status code.
		/// </summary>
		ResultType ResultType { get; set; }
		/// <summary>
		/// A description of the error if the action failed.
		/// </summary>
		string ErrorMessage { get;  }
		/// <summary>
		/// The resulting object instance if the action was successful.
		/// </summary>
		TResult Result { get; }
	}
}

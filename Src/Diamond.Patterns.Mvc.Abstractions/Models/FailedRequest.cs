namespace Diamond.Patterns.Mvc.Abstractions
{
	/// <summary>
	/// Provides detail for a failed request.
	/// </summary>
	public class FailedRequest
	{
		/// <summary>
		/// Creates a default instance of BadRequest.
		/// </summary>
		public FailedRequest()
		{
		}

		/// <summary>
		/// Creates an instance of BadRequest.
		/// </summary>
		/// <param name="code">The error code.</param>
		/// <param name="message">The error message.</param>
		/// <param name="innerError">The inner error.</param>
		public FailedRequest(string code, string message, Error innerError = null)
		{
			this.Error = new Error()
			{
				Code = code,
				Message = message,
				InnerError = innerError
			};
		}

		/// <summary>
		/// The resulting error from the bad request.
		/// </summary>
		public Error Error { get; set; }
	}
}

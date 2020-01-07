namespace Diamond.Patterns.Mvc.Abstractions
{
	/// <summary>
	/// Holds an error message from a bad request.
	/// </summary>
	public class Error
	{
		/// <summary>
		/// A code to identify this error.
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Description of an error that occurred.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// The error that is the cause of the current error, or a null reference
		/// if no inner error is specified.
		/// </summary>
		public Error InnerError { get; set; }
	}
}

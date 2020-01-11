namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Specifies the type of information represented by a log entry.
	/// </summary>
	public enum LoggingLevel
	{
		/// <summary>
		/// The entry is informational.
		/// </summary>
		Information,
		/// <summary>
		/// The entry indicates an warning has occurred. These entries
		/// are usually a result of unexpected behavior that does not
		/// prevent the application from performing in the desired manner.
		/// </summary>
		Warning,
		/// <summary>
		/// The entry indicates an error has occurred. These entries
		/// are usually a result o unexpected behavior that prevents
		/// the application from performing in the desired manner.
		/// </summary>
		Error,
		/// <summary>
		/// The entry indicates an error has occurred that has resulted
		/// in the suspension or termination of the application.
		/// </summary>
		Fatal,
		/// <summary>
		/// The entry is a debug message that contains useful information
		/// for troubleshooting an issue.
		/// </summary>
		Debug,
		/// <summary>
		/// The entry is verbose information that can be used to understand
		/// the detailed flow of an application.
		/// </summary>
		Verbose
	}
}

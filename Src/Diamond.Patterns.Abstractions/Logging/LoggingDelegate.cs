namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// A delegate for a callback method that can be used to obtain
	/// log messages from an object.
	/// </summary>
	/// <param name="loggingLevel">Specifies the type of information represented by a log entry.</param>
	/// <param name="message">The log message.</param>
	public delegate void LoggerDelegate(LoggingLevel loggingLevel, string message);
}

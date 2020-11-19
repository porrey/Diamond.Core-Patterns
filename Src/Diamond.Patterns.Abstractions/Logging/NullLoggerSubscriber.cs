namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Provides a Null Logger implementation.
	/// </summary>
	public class NullLoggerSubscriber : ILoggerSubscriber
	{
		/// <summary>
		/// Creates a new instance of <see cref="NullLoggerSubscriber"/>.
		/// </summary>
		public NullLoggerSubscriber()
		{
			this.Logger += this.OnInternalLogger;
		}

		/// <summary>
		/// Gets/sets the <see cref="LoggerDelegate"/>.
		/// </summary>
		public LoggerDelegate Logger { get; set; }

		/// <summary>
		/// Handles the event.
		/// </summary>
		/// <param name="loggingLevel">Specifies the type of information represented by a log entry.</param>
		/// <param name="message">Contains the log entry.</param>
		protected void OnInternalLogger(LoggingLevel loggingLevel, string message)
		{
		}
	}
}

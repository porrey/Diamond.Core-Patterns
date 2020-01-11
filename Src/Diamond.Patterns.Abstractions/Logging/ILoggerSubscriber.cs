namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Defines an interface for objects to be designated
	/// as a logger target.
	/// </summary>
	public interface ILoggerSubscriber
	{
		/// <summary>
		/// Gets/sets the delegate used to receive log events.
		/// </summary>
		LoggerDelegate Logger { get; set; }
	}
}

namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Defines an interface for objects to support logging.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Gets/sets the logger method for object instance.
		/// </summary>
		ILoggerSubscriber LoggerSubscriber { get; set; }
	}
}

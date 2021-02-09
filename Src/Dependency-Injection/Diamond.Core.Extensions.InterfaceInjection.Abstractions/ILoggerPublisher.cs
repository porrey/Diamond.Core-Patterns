using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.InterfaceInjection {
	/// <summary>
	/// 
	/// </summary>
	public interface ILoggerPublisher {
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ILoggerPublisher<T> : ILoggerPublisher {
		/// <summary>
		/// 
		/// </summary>
		ILogger<T> Logger { get; set; }
	}
}

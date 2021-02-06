using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.InterfaceInjection
{
	public interface ILoggerPublisher
	{
	}

	public interface ILoggerPublisher<T> : ILoggerPublisher
	{
		ILogger<T> Logger { get; set; }
	}
}

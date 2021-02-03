using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.DependencyInjection
{
	public interface ILoggerPublisher
	{
	}

	public interface ILoggerPublisher<T> : ILoggerPublisher
	{
		ILogger<T> Logger { get; set; }
	}
}

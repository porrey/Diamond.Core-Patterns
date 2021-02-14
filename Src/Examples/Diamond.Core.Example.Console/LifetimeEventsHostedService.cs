using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	internal class LifetimeEventsHostedService : IHostedService
	{
		private readonly ILogger _logger;
		private readonly IHostApplicationLifetime _appLifetime;

		public LifetimeEventsHostedService(
			ILogger<LifetimeEventsHostedService> logger,
			IHostApplicationLifetime appLifetime)
		{
			_logger = logger;
			_appLifetime = appLifetime;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_appLifetime.ApplicationStarted.Register(OnStarted);
			_appLifetime.ApplicationStopping.Register(OnStopping);
			_appLifetime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private void OnStarted()
		{
			_logger.LogInformation("OnStarted has been called.");
		}

		private void OnStopping()
		{
			_logger.LogInformation("OnStopping has been called.");
		}

		private void OnStopped()
		{
			_logger.LogInformation("OnStopped has been called.");
		}
	}
}

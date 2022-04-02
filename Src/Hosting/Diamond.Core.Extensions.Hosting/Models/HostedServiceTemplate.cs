using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Extensions.Hosting
{
	/// <summary>
	/// 
	/// </summary>
	public class HostedServiceTemplate : IHostedService
	{
		/// <summary>
		/// 
		/// </summary>
		public HostedServiceTemplate(IHostApplicationLifetime hostApplicationLifetime)
		{
			this.HostApplicationLifetime = hostApplicationLifetime;
			this.Logger.LogDebug("Initialized instance of '{object}'.", nameof(HostedServiceTemplate));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="hostApplicationLifetime"></param>
		public HostedServiceTemplate(IHostApplicationLifetime hostApplicationLifetime, ILogger<HostedServiceTemplate> logger)
			: this(hostApplicationLifetime)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="hostApplicationLifetime"></param>
		/// <param name="serviceScopeFactory"></param>
		public HostedServiceTemplate(IHostApplicationLifetime hostApplicationLifetime, ILogger<HostedServiceTemplate> logger, IServiceScopeFactory serviceScopeFactory)
			: this(hostApplicationLifetime, logger)
		{
			this.ServiceScopeFactory = serviceScopeFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual IHostApplicationLifetime HostApplicationLifetime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected virtual ILogger<HostedServiceTemplate> Logger { get; set; } = new NullLogger<HostedServiceTemplate>();

		/// <summary>
		/// 
		/// </summary>
		protected virtual IServiceScopeFactory ServiceScopeFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected virtual CancellationToken CancellationToken { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public virtual async Task StartAsync(CancellationToken cancellationToken)
		{
			await this.OnBeginStartAsync();
			
			this.Logger.LogDebug("The service {name} has been started.", nameof(HostedServiceTemplate));
			this.CancellationToken = cancellationToken;
			this.Logger.LogDebug("Registering 'ApplicationStarted' in {name} Service.", nameof(HostedServiceTemplate));
			this.HostApplicationLifetime.ApplicationStarted.Register(this.OnStarted);
			this.Logger.LogDebug("Registering 'ApplicationStopping' in {name} Service.", nameof(HostedServiceTemplate));
			this.HostApplicationLifetime.ApplicationStopping.Register(this.OnStopping);
			this.Logger.LogDebug("Registering 'ApplicationStopped' in {name} Service.", nameof(HostedServiceTemplate));
			this.HostApplicationLifetime.ApplicationStopped.Register(this.OnStopped);

			await this.OnCompletedStartAsync();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual Task OnBeginStartAsync()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual Task OnCompletedStartAsync()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public virtual async Task StopAsync(CancellationToken cancellationToken)
		{
			await this.OnBeginStopAsync();
			this.Logger.LogDebug("The service {name} has been stopped.", nameof(HostedServiceTemplate));
			await this.OnCompletedStopAsync();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual Task OnBeginStopAsync()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual Task OnCompletedStopAsync()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnStopped()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnStopping()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnStarted()
		{
		}
	}
}

using System;
using System.Threading;
using System.Windows;
using Diamond.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Wpf
{
	/// <summary>
	/// 
	/// </summary>
	public class HostedApplication : Application
	{
		/// <summary>
		/// 
		/// </summary>
		protected IHost ApplicationHost { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected CancellationTokenSource CancellationToken { get; } = new CancellationTokenSource();

		/// <summary>
		/// 
		/// </summary>
		public HostedApplication()
			: base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnStartup(StartupEventArgs e)
		{
			try
			{
				this.OnBeginStartup(e);

				//
				//
				//
				this.ApplicationHost = this.OnCreateHost();

				//
				// Get the main window.
				//
				IWindow startupWindow = this.OnGetStartupWindow(this.ApplicationHost.Services);

				if (startupWindow != null)
				{
					startupWindow.Show();
				}

				//
				// Start the host.
				//
				this.ApplicationHost.RunAsync(this.CancellationToken.Token);
			}
			finally
			{
				this.OnCompletedStartup(e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBeginStartup(StartupEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnCompletedStartup(StartupEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual IHost OnCreateHost()
		{
			IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
										.ConfigureHostConfiguration((configurationBuilder) =>
										{
											this.OnConfigureHostConfiguration(configurationBuilder);
										})
										.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
										{
											this.OnConfigureAppConfiguration(hostContext, configurationBuilder);
										})
										.ConfigureLogging((hostContext, loggingBuilder) =>
										{
											this.OnConfigureLogging(hostContext, loggingBuilder);
										})
										.ConfigureServices((hostContext, services) =>
										{
											this.OnConfigureServices(hostContext, services);
										})
										.UseConfiguredServices();

			return this.OnConfigureHost(hostBuilder).Build();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder"></param>
		/// <returns></returns>
		protected virtual IHostBuilder OnConfigureHost(IHostBuilder hostBuilder)
		{
			return hostBuilder;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		protected virtual IWindow OnGetStartupWindow(IServiceProvider serviceProvider)
		{
			//
			// Returns null if not found.
			//
			return serviceProvider.GetService<IMainWindow>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurationBuilder"></param>
		protected virtual void OnConfigureHostConfiguration(IConfigurationBuilder configurationBuilder)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostContext"></param>
		/// <param name="configurationBuilder"></param>
		protected virtual void OnConfigureAppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder configurationBuilder)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder"></param>
		/// <param name="loggingBuilder"></param>
		protected virtual void OnConfigureLogging(HostBuilderContext hostBuilder, ILoggingBuilder loggingBuilder)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostContext"></param>
		/// <param name="services"></param>
		protected virtual void OnConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
		{
		}
	}
}

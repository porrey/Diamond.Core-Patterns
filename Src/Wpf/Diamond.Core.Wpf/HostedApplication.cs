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
	/// Encapsulates a Windows Presentation Foundation application that
	/// using the hosting model.
	/// </summary>
	public class HostedApplication : Application
	{
		/// <summary>
		/// Creates a default instance of <see cref="HostedApplication"/>.
		/// </summary>
		public HostedApplication()
			: base()
		{
		}

		/// <summary>
		/// Gets/sets the <see cref="Host"/> instance.
		/// </summary>
		protected IHost ApplicationHost { get; set; }

		/// <summary>
		/// Signals to a <see cref="CancellationToken"/> that it should be canceled.
		/// </summary>
		protected CancellationTokenSource CancellationToken { get; } = new CancellationTokenSource();

		/// <summary>
		/// The application startup event.
		/// </summary>
		/// <param name="e">A <see cref="StartupEventArgs"/> that contains the event data.</param>
		protected sealed override void OnStartup(StartupEventArgs e)
		{
			try
			{
				this.OnBeginStartup(e);

				//
				// Create the host.
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
		/// Called at the beginning of the application startup process.
		/// </summary>
		/// <param name="e">A <see cref="StartupEventArgs"/> that contains the event data.</param>
		protected virtual void OnBeginStartup(StartupEventArgs e)
		{
		}

		/// <summary>
		/// Called at the end of the application startup process.
		/// </summary>
		/// <param name="e">A <see cref="StartupEventArgs"/> that contains the event data.</param>
		protected virtual void OnCompletedStartup(StartupEventArgs e)
		{
		}

		/// <summary>
		/// Called to create and build the <see cref="Host"/> instance. Override
		/// this method to replace or customize the host creation. The default
		/// implementation calls the internal methods during host creation.
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
		/// This method is called just prior to the build method so the host can be 
		/// modified before being built.
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder"/> instance being constructed.</param>
		/// <returns>The <see cref="IHostBuilder"/> that can be used for chaining calls.</returns>
		protected virtual IHostBuilder OnConfigureHost(IHostBuilder hostBuilder)
		{
			return hostBuilder;
		}

		/// <summary>
		/// This method is called to get the startup view. The default implementation retrieves
		/// the IMainWindow registered in the container. Override this method to provide a
		/// custom view.
		/// </summary>
		/// <param name="serviceProvider">An instance of <see cref="IServiceProvider"/>.</param>
		/// <returns>Returns an IWindow instance that will be used as the first or primary view.</returns>
		protected virtual IWindow OnGetStartupWindow(IServiceProvider serviceProvider)
		{
			//
			// Returns null if not found.
			//
			return serviceProvider.GetService<IMainWindow>();
		}

		/// <summary>
		/// Set up the configuration for the builder itself. This will be used to initialize
		/// the <see cref="IHostEnvironment"/> for use later in the build
		/// process. This can be called multiple times and the results will be additive.
		/// </summary>
		/// <param name="configurationBuilder">The instance of <see cref="IConfigurationBuilder"/> used during host creation.</param>
		protected virtual void OnConfigureHostConfiguration(IConfigurationBuilder configurationBuilder)
		{
		}

		/// <summary>
		/// Sets up the configuration for the remainder of the build process and application.
		/// </summary>
		/// <param name="hostContext">The instance of <see cref="HostBuilderContext"/> used in the build process.</param>
		/// <param name="configurationBuilder">The instance of <see cref="IConfigurationBuilder"/> used in the build process.</param>
		protected virtual void OnConfigureAppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder configurationBuilder)
		{
		}

		/// <summary>
		/// Setup the logging configuration during the host build process.
		/// </summary>
		/// <param name="hostBuilder">The instance of <see cref="HostBuilderContext"/> used in the build process.</param>
		/// <param name="loggingBuilder">The instance of <see cref="ILoggingBuilder"/> used in the build process.</param>
		protected virtual void OnConfigureLogging(HostBuilderContext hostBuilder, ILoggingBuilder loggingBuilder)
		{
		}

		/// <summary>
		/// Setup services during the host build process.
		/// </summary>
		/// <param name="hostContext">The instance of <see cref="HostBuilderContext"/> used in the build process.</param>
		/// <param name="services">The instance of <see cref="IServiceCollection"/> used in the build process.</param>
		protected virtual void OnConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
		{
		}
	}
}

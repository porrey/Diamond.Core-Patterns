using Diamond.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Wpf
{
	/// <summary>
	/// Encapsulates a Windows Presentation Foundation application that
	/// using the hosting model and a custom container.
	/// </summary>
	/// <typeparam name="TContainerBuilder"></typeparam>
	public class HostedApplication<TContainerBuilder> : HostedApplication
	{
		/// <summary>
		/// Called to create and build the <see cref="Host"/> instance. Override
		/// this method to replace or customize the host creation. The default
		/// implementation calls the internal methods during host creation including
		/// a call to setup the custom container.
		/// </summary>
		/// <returns></returns>
		protected override IHost OnCreateHost()
		{
			return Host.CreateDefaultBuilder()
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
						.ConfigureContainer<TContainerBuilder>((hostContext, container) =>
						{
							this.OnConfigureContainer(hostContext, container);
						})
						.UseConfiguredServices()
						.Build();
		}

		/// <summary>
		/// Setup a custom container during the host build process.
		/// </summary>
		/// <param name="hostContext">The instance of <see cref="HostBuilderContext"/> used in the build process.</param>
		/// <param name="container">An instance of the customer container of type TContainerBuilder.</param>
		protected virtual void OnConfigureContainer(HostBuilderContext hostContext, TContainerBuilder container)
		{
		}
	}
}

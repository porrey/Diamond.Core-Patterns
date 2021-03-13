using Diamond.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Wpf
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TContainerBuilder"></typeparam>
	public class HostedApplication<TContainerBuilder> : HostedApplication
	{
		/// <summary>
		/// 
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
		/// 
		/// </summary>
		/// <param name="hostContext"></param>
		/// <param name="container"></param>
		protected virtual void OnConfigureContainer(HostBuilderContext hostContext, TContainerBuilder container)
		{
		}
	}
}

using System.CommandLine;
using Diamond.Core.ConsoleCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.ConsoleCommand
{
	/// <summary>
	/// 
	/// </summary>
	public class ConsoleStartup : IStartupConfigureServices, IStartupAppConfiguration, IStartupConfigureLogging
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		public void ConfigureAppConfiguration(IConfigurationBuilder builder)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		public void ConfigureLogging(ILoggingBuilder builder)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public IServiceCollection ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<ICommand, HelloCommand>();
			services.AddTransient<ICommand, EchoCommand>();
			return services;
		}
	}
}

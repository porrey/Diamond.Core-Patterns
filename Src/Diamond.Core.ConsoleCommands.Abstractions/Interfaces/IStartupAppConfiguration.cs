using Microsoft.Extensions.Configuration;

namespace Diamond.Core.ConsoleCommands
{
	public interface IStartupAppConfiguration : IStartup
	{
		void ConfigureAppConfiguration(IConfigurationBuilder builder);
	}
}

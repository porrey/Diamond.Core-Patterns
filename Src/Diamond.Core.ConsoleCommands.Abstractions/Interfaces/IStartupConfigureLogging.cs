using Microsoft.Extensions.Logging;

namespace Diamond.Core.ConsoleCommands
{
	public interface IStartupConfigureLogging : IStartup
	{
		void ConfigureLogging(ILoggingBuilder builder);
	}
}

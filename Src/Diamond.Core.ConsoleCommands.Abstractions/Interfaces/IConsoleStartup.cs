using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.ConsoleCommands
{
	public interface IStartupConfigureServices : IStartup
	{
		IServiceCollection ConfigureServices(IServiceCollection services);
	}
}

using System.CommandLine;
using System.Threading.Tasks;
using Diamond.Core.ConsoleCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example.ConsoleCommand
{
	class Program
	{
		static async Task Main(string[] args) => await ConsoleHost.CreateRootCommand("Sample Application", args)
				.UseDiamondCoreHost(args, services => ConfigureMyServices(services))
				.RunConsoleAsync();

		private static IServiceCollection ConfigureMyServices(IServiceCollection services)
		{
			services.AddTransient<ICommand, HelloCommand>();
			services.AddTransient<ICommand, EchoCommand>();
			return services;
		}
	}
}

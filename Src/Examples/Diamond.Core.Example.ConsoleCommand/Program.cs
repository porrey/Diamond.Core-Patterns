using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Diamond.Core.ConsoleCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example.ConsoleCommand
{
	class Program
	{
		static async Task Main(string[] args) => await BuildCommandLine()
			.UseDiamondCommandPattern(RootCommand, (e) => Host.CreateDefaultBuilder(e),
				host =>
				{
					host.ConfigureServices(services =>
					{
						ConfigureMyServices(services);
					});
				})
			.UseDefaults()
			.Build()
			.InvokeAsync(args);

		private static RootCommand RootCommand = new RootCommand("Sample Command Application");

		private static CommandLineBuilder BuildCommandLine()=> new CommandLineBuilder(RootCommand);

		private static IServiceCollection ConfigureMyServices(IServiceCollection services)
		{
			services.AddTransient<ICommand, HelloCommand>();
			return services;
		}
	}
}

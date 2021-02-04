using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Diamond.Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.Command.Console
{
	class Program
	{
		static async Task Main(string[] args) => await BuildCommandLine()
			.UseDiamondCommandPattern((e) => Host.CreateDefaultBuilder(e),
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

		private static CommandLineBuilder BuildCommandLine()
		{
			RootCommand rootCommand = new RootCommand("Sample Command Application");
			System.CommandLine.Command command = new System.CommandLine.Command("test", "Test Command");

			Option<int> option = new Option<int>("--id", "Employee ID")
			{
				IsRequired = true
			};

			command.AddOption(option);

			command.Handler = CommandHandler.Create<IHost, Employee>((h, e) =>
			{

			});

			rootCommand.AddCommand(command);

			return new CommandLineBuilder(rootCommand);
		}

		private static IServiceCollection ConfigureMyServices(IServiceCollection services)
		{
			return services;
		}
	}
}

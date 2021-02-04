using System.CommandLine;
using System.CommandLine.Builder;
using System.Threading.Tasks;
using Diamond.Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example.Command.Console
{
	class Program
	{
		//public static IHostBuilder CreateConsoleBuilder(string[] args) =>
		//		Host.CreateDefaultBuilder(args)
		//			.UseConsoleLifetime()
		//			.ConfigureServices(services => Program.ConfigureServices(services));

		static Task Main(string[] args)
		{
			// ***
			// *** Run as console application
			// ***
			CommandLineBuilder builder = new CommandLineBuilder();
			builder.UseDiamondCommandPattern(args, _ => Host.CreateDefaultBuilder()
															.ConfigureServices(services => Program.ConfigureServices(services)),
														(commandLineBuilder) =>
														{
															RootCommand rootCommand = new RootCommand("Sample Command Application");

															System.CommandLine.Command command = new System.CommandLine.Command("test", "Test Command");
															command.AddArgument(new Argument<string>("--name", "Full name of employee"));
															rootCommand.AddCommand(command);

															commandLineBuilder.AddCommand(rootCommand);
														});

			var parser = builder.Build();

			return Task.FromResult(0);
		}

		//static async Task Main(string[] args) => await BuildCommandLine()
		//	.UseHost
		//	(
		//		_ => Host.CreateDefaultBuilder()
		//				 .ConfigureServices(services => Program.ConfigureServices(services))
		//	)
		//	.UseDefaults()
		//	.Build()
		//	.InvokeAsync(args);

		private static void ConfigureServices(IServiceCollection services)
		{

		}

		private static void Run(IHost host)
		{

		}
	}
}

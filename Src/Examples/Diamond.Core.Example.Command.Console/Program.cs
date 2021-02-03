using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Collections;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example.Command.Console
{
	class Program
	{
		static async Task Main(string[] args) => await BuildCommandLine()
			.UseHost
			(
				_ => Host.CreateDefaultBuilder()
						 .ConfigureServices(services => Program.ConfigureServices(services))
			)
			.UseMiddleware(async (invocation, next) =>
			{
				await next(invocation);
			})
			.UseDefaults()
			.Build()
			.InvokeAsync(args);

		private static void ConfigureServices(IServiceCollection services)
		{
		}

		private static CommandLineBuilder BuildCommandLine()
		{
			RootCommand root = new RootCommand(@"$ dotnet run --name 'Joe'")
			{
				new Option<string>("--name")
				{
					IsRequired = true
				}
			};

			root.Handler = CommandHandler.Create<IHost>(Run);

			return new CommandLineBuilder(root);
		}

		private static void Run(IHost host)
		{

		}
	}
}

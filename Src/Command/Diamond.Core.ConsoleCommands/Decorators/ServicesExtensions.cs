using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.ConsoleCommands
{
	public class RootCommandService : RootCommand, IHostedService
	{
		public RootCommandService(string description, string[] args)
			: base(description)
		{
			this.Args = args;
		}

		protected string[] Args { get; set; }

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			int result = await this.InvokeAsync(this.Args);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}

	public static class ConsoleHost
	{
		public static RootCommandService CreateRootCommand(string name, string[] args)
		{
			return new RootCommandService(name, args);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public static class ServicesExtensions
	{
		public static IHostBuilder UseDiamondCoreHost(this RootCommandService rootCommand, string[] args, Action<IServiceCollection> configureServices)
		{
			// ***
			// ***
			// ***
			IHostBuilder builder = Host.CreateDefaultBuilder(args);

			// ***
			// ***
			// ***
			IHostBuilder services = builder.ConfigureServices(services =>
			{
				// ***
				// ***
				// ***
				configureServices(services);

				// ***
				// ***
				// ***
				ServiceProvider sp = services.BuildServiceProvider();

				// ***
				// *** Get any commands configured.
				// ***
				IEnumerable<ICommand> commands = sp.GetRequiredService<IEnumerable<ICommand>>();

				if (commands.Any())
				{
					foreach (ICommand command in commands)
					{
						if (command is Command cmd)
						{
							rootCommand.AddCommand(cmd);
						}
					}
				}

				// ***
				// ***
				// ***
				services.AddHostedService(_ => rootCommand);
			});


			return builder;
		}
	}
}

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
	/// <summary>
	/// 
	/// </summary>
	public static class ServicesExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rootCommand"></param>
		/// <param name="args"></param>
		/// <param name="configureServices"></param>
		/// <returns></returns>
		public static IHostBuilder UseDiamondCoreHost<TStartup>(this RootCommandService rootCommand, string[] args)
			where TStartup : IStartup, new()
		{
			// ***
			// *** 
			// ***
			IHostBuilder builder = Host.CreateDefaultBuilder(args);

			// ***
			// ***
			// ***
			IStartup startup = new TStartup();

			// ***
			// ***
			// ***
			if (startup is IStartupAppConfiguration startupAppConfiguration)
			{
				builder.ConfigureAppConfiguration(builder => startupAppConfiguration.ConfigureAppConfiguration(builder));
			}

			// ***
			// ***
			// ***
			if (startup is IStartupConfigureLogging startupConfigureLogging)
			{
				builder.ConfigureLogging(builder => startupConfigureLogging.ConfigureLogging(builder));
			}

			// ***
			// ***
			// ***
			IHostBuilder services = builder.ConfigureServices(services =>
			{
				// ***
				// ***
				// ***
				if (startup is IStartupConfigureServices startupConfigureServices)
				{
					startupConfigureServices.ConfigureServices(services);
				}

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

		/// <summary>
		/// Enables console support, builds and starts the host, and waits for Ctrl+C or SIGTERM to shut down.
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Returns the exit code set in the environemnt.</returns>
		public static async Task<int> RunCommandAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default)
		{
			await hostBuilder.UseConsoleLifetime().Build().RunAsync(cancellationToken);
			return Environment.ExitCode;
		}
	}
}

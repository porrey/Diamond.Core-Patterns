using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
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

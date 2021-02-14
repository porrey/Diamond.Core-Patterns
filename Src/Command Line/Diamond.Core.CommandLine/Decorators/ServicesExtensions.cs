//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.CommandLine
{
	/// <summary>
	/// 
	/// </summary>
	public static class ServicesExtensions
	{
		/// <summary>
		/// Create a root comand object that wraps a host builder. The host builder
		/// is create using defaults.
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <param name="name">The name of the root command.</param>
		/// <param name="args">The arguments passed to the application at the console prompt.</param>
		/// <returns>A <see cref="IHostBuilder"/> instance used to configure and build the application.</returns>
		public static IHostBuilder UseRootCommand(this IHostBuilder hostBuilder, string name, string[] args)
		{
			//
			// Create the root command/service for this application. Only
			// one instance is needed.
			//
			InternalRootCommand rootCommand = new InternalRootCommand(name, args);

			//
			// Add the root command to the services.
			//
			hostBuilder.ConfigureServices(services =>
			{
				services.AddSingleton<IRootCommand>(rootCommand);
			});

			return hostBuilder;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns></returns>
		public static IHostBuilder UseCommands(this IHostBuilder hostBuilder)
		{
			//
			// Configure the command services.
			//
			IHostBuilder services = hostBuilder.ConfigureServices(services =>
			{
				//
				// Build the services now to gain access to the needed componetns.
				//
				ServiceProvider sp = services.BuildServiceProvider();

				//
				// Get a logger.
				//
				ILogger<IHostBuilder> logger = sp.GetRequiredService<ILogger<IHostBuilder>>();

				//
				// Get any commands configured.
				//
				IEnumerable<ICommand> commands = sp.GetRequiredService<IEnumerable<ICommand>>();

				if (commands.Any())
				{
					//
					// Get the root command
					//
					IRootCommand rootCommand = sp.GetRequiredService<IRootCommand>();

					foreach (ICommand cmd in commands)
					{
						if (cmd is Command c)
						{
							logger.LogDebug($"Loading '{c.Name}' command.");
							((RootCommand)rootCommand).AddCommand(c);
						}
					}
				}

				//
				// Add the root command service as the hosted service so that
				// it will get executed when all the setup has completed.
				//
				logger.LogDebug($"Adding Root Command Host Service.");
				services.AddHostedService<RootCommandService>();
			});

			return hostBuilder;
		}
	}
}

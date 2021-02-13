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
		/// <typeparam name="TStartup"></typeparam>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static IHostBuilder UseDiamondCoreHost<TStartup>(this IHostBuilder builder)
			where TStartup : IStartup, new()
		{
			//
			// Create an instance of the start up class.
			//
			IStartup startup = new TStartup();

			//
			// Call the startup object's ConfigureAppConfiguration method.
			//
			if (startup is IStartupAppConfiguration startupAppConfiguration)
			{
				builder.ConfigureAppConfiguration(builder => startupAppConfiguration.ConfigureAppConfiguration(builder));
			}

			//
			// Call the startup object's ConfigureLogging method.
			//
			if (startup is IStartupConfigureLogging startupConfigureLogging)
			{
				builder.ConfigureLogging(builder => startupConfigureLogging.ConfigureLogging(builder));
			}

			//
			// Configure the command services.
			//
			IHostBuilder services = builder.ConfigureServices(services =>
			{
				//
				// Call the startup object's ConfigureServices method.
				//
				if (startup is IStartupConfigureServices startupConfigureServices)
				{
					startupConfigureServices.ConfigureServices(services);
				}

				//
				// Build the services now to gain access to the needed componetns.
				//
				ServiceProvider sp = services.BuildServiceProvider();

				//
				// Get any commands configured.
				//
				IEnumerable<ICommand> commands = sp.GetRequiredService<IEnumerable<ICommand>>();

				//
				// Get the root command
				//
				IRootCommandService rootCommandService = sp.GetRequiredService<IRootCommandService>();

				if (rootCommandService is RootCommand rootCommand)
				{
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

					//
					// Add the root command as the hosted service so that
					// it will get executed when all the setup has completed.
					//
					services.AddHostedService(_ => rootCommandService);
				}
			});

			return builder;
		}
	}
}

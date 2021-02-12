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
		/// <param name="rootCommand"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IHostBuilder UseDiamondCoreHost<TStartup>(this RootCommandService rootCommand, string[] args)
			where TStartup : IStartup, new()
		{
			//
			// 
			//
			IHostBuilder builder = Host.CreateDefaultBuilder(args);

			//
			//
			//
			IStartup startup = new TStartup();

			//
			//
			//
			if (startup is IStartupAppConfiguration startupAppConfiguration)
			{
				builder.ConfigureAppConfiguration(builder => startupAppConfiguration.ConfigureAppConfiguration(builder));
			}

			//
			//
			//
			if (startup is IStartupConfigureLogging startupConfigureLogging)
			{
				builder.ConfigureLogging(builder => startupConfigureLogging.ConfigureLogging(builder));
			}

			//
			//
			//
			IHostBuilder services = builder.ConfigureServices(services =>
			{
				//
				//
				//
				if (startup is IStartupConfigureServices startupConfigureServices)
				{
					startupConfigureServices.ConfigureServices(services);
				}

				//
				//
				//
				ServiceProvider sp = services.BuildServiceProvider();

				//
				// Get any commands configured.
				//
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

				//
				//
				//
				services.AddHostedService(_ => rootCommand);
			});

			return builder;
		}
	}
}

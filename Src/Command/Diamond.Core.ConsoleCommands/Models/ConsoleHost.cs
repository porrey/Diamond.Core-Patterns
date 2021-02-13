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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.ConsoleCommands
{
	/// <summary>
	/// 
	/// </summary>
	public static class ConsoleHost
	{
		/// <summary>
		/// Create a root comand object that wraps a host builder. the host builder
		/// is create using defaults.
		/// </summary>
		/// <param name="name">The name of the root command.</param>
		/// <param name="args">The arguments passed to the application at the console prompt.</param>
		/// <returns>A <see cref="IHostBuilder"/> instance used to configure and build the application.</returns>
		public static IHostBuilder CreateRootCommand(string name, string[] args)
		{
			//
			// Create the root command/service for this application. Only
			// one instance is needed.
			//
			RootCommandService root = new RootCommandService(name, args);

			//
			// Create the default builder.
			//
			IHostBuilder builder = Host.CreateDefaultBuilder(args);

			//
			// Add the root command to the services.
			//
			builder.ConfigureServices(services =>
			{
				services.AddSingleton<IRootCommandService>(root);
			});
			
			return builder;
		}
	}
}

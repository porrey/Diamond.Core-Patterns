﻿//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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

namespace Diamond.Core.CommandLine
{
	/// <summary>
	/// 
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// Create a root command object that wraps a host builder. The host builder
		/// is create using defaults.
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <param name="name">The name of the root command.</param>
		/// <param name="args">The arguments passed to the application at the console prompt.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
		public static IHostBuilder AddRootCommand(this IHostBuilder hostBuilder, string name, string[] args)
		{
			//
			// Create the root command/service for this application. Only
			// one instance is needed.
			//
			InternalRootCommand rootCommand = new(name, args);

			//
			// Add the root command to the services.
			//
			hostBuilder.ConfigureServices(services =>
			{
				services.AddSingleton<IRootCommand>(rootCommand);
				services.AddHostedService<RootCommandService>();
			});

			return hostBuilder;
		}
	}
}

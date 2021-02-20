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
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.Hosting
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
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns></returns>
		public static IHostBuilder UseStartup<TStartup>(this IHostBuilder hostBuilder)
			where TStartup : IStartup, new()
		{
			//
			//
			//
			IStartup startup = new TStartup();

			//
			//
			//
			if (startup is IStartupAppConfiguration startupAppConfiguration)
			{
				hostBuilder.ConfigureAppConfiguration((context, builder) =>
				{
					//
					//
					//
					startupAppConfiguration.ConfigureAppConfiguration(builder);

					//
					//
					//
					if (startup is IStartupConfiguration startupConfiguration)
					{
						startupConfiguration.Configuration = context.Configuration;
					}
				});
			}

			//
			//
			//
			if (startup is IStartupConfigureLogging startupConfigureLogging)
			{
				hostBuilder.ConfigureLogging(builder => startupConfigureLogging.ConfigureLogging(builder));
			}

			//
			//
			//
			if (startup is IStartupConfigureServices startupConfigureServices)
			{
				hostBuilder.ConfigureServices(services => startupConfigureServices.ConfigureServices(services));
			}

			return hostBuilder;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TStartup"></typeparam>
		/// <typeparam name="TContainer"></typeparam>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns></returns>
		public static IHostBuilder UseStartup<TStartup, TContainer>(this IHostBuilder hostBuilder)
			where TStartup : IStartup, new()
		{
			//
			//
			//
			IStartup startup = new TStartup();

			//
			//
			//
			if (startup is IStartupAppConfiguration startupAppConfiguration)
			{
				hostBuilder.ConfigureAppConfiguration((context, builder) =>
				{
					//
					//
					//
					if (startup is IStartupConfiguration startupConfiguration)
					{
						startupConfiguration.Configuration = context.Configuration;
					}

					//
					//
					//
					startupAppConfiguration.ConfigureAppConfiguration(builder);
				});
			}

			//
			//
			//
			if (startup is IStartupConfigureLogging startupConfigureLogging)
			{
				hostBuilder.ConfigureLogging(builder => startupConfigureLogging.ConfigureLogging(builder));
			}

			//
			//
			//
			if (startup is IStartupConfigureServices startupConfigureServices)
			{
				hostBuilder.ConfigureServices(services => startupConfigureServices.ConfigureServices(services));
			}

			//
			//
			//
			if (startup is IStartupConfigureContainer startupConfigureContainer)
			{
				hostBuilder.ConfigureContainer<TContainer>(container => startupConfigureContainer.ConfigureContainer<TContainer>(container));
			}

			return hostBuilder;
		}

		/// <summary>
		/// Enables console support, builds and starts the host, and waits for Ctrl+C or SIGTERM to shut down.
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <param name="cancellationToken">The token to trigger shutdown.</param>
		/// <returns>Returns the exit code set in the environment.</returns>
		public static async Task<int> BuildAndRunWithExitCodeAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default)
		{
			await hostBuilder.UseConsoleLifetime().Build().RunAsync(cancellationToken);
			return Environment.ExitCode;
		}

		/// <summary>
		/// Runs an application and returns a Task that only completes when the token is
		/// triggered or shutdown is triggered.
		/// </summary>
		/// <param name="host">The Microsoft.Extensions.Hosting.IHost to run.</param>
		/// <param name="cancellationToken">The token to trigger shutdown.</param>
		/// <returns>Returns the exit code set in the environment.</returns>
		public static async Task<int> RunWithExitCodeAsync(this IHost host, CancellationToken cancellationToken = default)
		{
			await host.RunAsync(cancellationToken);
			return Environment.ExitCode;
		}
	}
}

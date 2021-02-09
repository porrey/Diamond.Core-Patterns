using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.Hosting {
	/// <summary>
	/// 
	/// </summary>
	public static class ServicesExtensions {
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TStartup"></typeparam>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static IHostBuilder UseStartup<TStartup>(this IHostBuilder builder)
			where TStartup : IStartup, new() {
			//
			//
			//
			IStartup startup = new TStartup();

			//
			//
			//
			if (startup is IStartupAppConfiguration startupAppConfiguration) {
				builder.ConfigureAppConfiguration(builder => startupAppConfiguration.ConfigureAppConfiguration(builder));
			}

			//
			//
			//
			if (startup is IStartupConfigureLogging startupConfigureLogging) {
				builder.ConfigureLogging(builder => startupConfigureLogging.ConfigureLogging(builder));
			}

			//
			//
			//
			if (startup is IStartupConfigureServices startupConfigureServices) {
				builder.ConfigureServices(services => startupConfigureServices.ConfigureServices(services));
			}

			return builder;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TStartup"></typeparam>
		/// <typeparam name="TContainer"></typeparam>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static IHostBuilder UseStartup<TStartup, TContainer>(this IHostBuilder builder)
			where TStartup : IStartup, new() {
			//
			//
			//
			IStartup startup = new TStartup();

			//
			//
			//
			if (startup is IStartupAppConfiguration startupAppConfiguration) {
				builder.ConfigureAppConfiguration(builder => startupAppConfiguration.ConfigureAppConfiguration(builder));
			}

			//
			//
			//
			if (startup is IStartupConfigureLogging startupConfigureLogging) {
				builder.ConfigureLogging(builder => startupConfigureLogging.ConfigureLogging(builder));
			}

			//
			//
			//
			if (startup is IStartupConfigureServices startupConfigureServices) {
				builder.ConfigureServices(services => startupConfigureServices.ConfigureServices(services));
			}

			//
			//
			//
			if (startup is IStartupConfigureContainer startupConfigureContainer) {
				builder.ConfigureContainer<TContainer>(container => startupConfigureContainer.ConfigureContainer<TContainer>(container));
			}

			return builder;
		}

		/// <summary>
		/// Enables console support, builds and starts the host, and waits for Ctrl+C or SIGTERM to shut down.
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Returns the exit code set in the environemnt.</returns>
		public static async Task<int> RunCommandAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default) {
			await hostBuilder.UseConsoleLifetime().Build().RunAsync(cancellationToken);
			return Environment.ExitCode;
		}
	}
}

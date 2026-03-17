//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Configures the application to load configuration settings from a specified folder.
	/// </summary>
	/// <remarks>This method adds the specified folder as a source of configuration settings for the application.
	/// The configuration files in the folder are loaded with the default settings: they are optional and do not reload on
	/// change.</remarks>
	public static class ServicesHostBuilderExtensions
	{
		/// <summary>
		/// Configures the application to load configuration settings from a specified folder.
		/// </summary>
		/// <remarks>This method adds configuration settings from the specified folder to the application's
		/// configuration. The configuration files in the folder are treated as optional and are not reloaded automatically
		/// when changed.</remarks>
		/// <param name="hostBuilder">The <see cref="IHostBuilder"/> instance to configure.</param>
		/// <param name="folderPath">The path to the folder containing configuration files. The folder path can be relative or absolute.</param>
		/// <param name="provider">An optional <see cref="IFileProvider"/> to use for accessing the configuration files. If not provided, the default file provider will be used.</param>
		/// <param name="optional"><see langword="true"/> to treat the folder as optional, meaning that if the folder does not exist, no error will 
		/// be thrown; otherwise, <see langword="false"/>. The default value is <see langword="true"/>.</param>
		/// <param name="reloadOnChange"><see langword="true"/> to enable automatic reloading of configuration when files in the folder change; 
		/// otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</param>
		/// <returns>The <see cref="IHostBuilder"/> instance with the configuration applied.</returns>
		public static IHostBuilder ConfigureServicesFolder(this IHostBuilder hostBuilder, string folderPath, IFileProvider provider = null, bool optional = true, bool reloadOnChange = false)
		{
			return hostBuilder.ConfigureAppConfiguration(config =>
			{
				config.AddServicesConfigurationFolder(folderPath: folderPath, provider: provider, optional: optional, reloadOnChange: reloadOnChange);
			});
		}

		/// <summary>
		/// Configures the host builder to load application services from a configuration file.
		/// </summary>
		/// <remarks>Use this method to add service configuration from an external file to the application's
		/// configuration pipeline. This enables dynamic service registration based on file contents and supports optional
		/// file presence.</remarks>
		/// <param name="hostBuilder">The host builder to configure with the services configuration file.</param>
		/// <param name="filePath">The path to the configuration file containing service definitions. The file is optional; if not found, no error is
		/// thrown.</param>
		/// <param name="provider">An optional <see cref="IFileProvider"/> to use for accessing the configuration files. If not provided, the default file provider will be used.</param>

		/// <param name="optional"><see langword="true"/> to treat the folder as optional, meaning that if the folder does not exist, no error will 
		/// be thrown; otherwise, <see langword="false"/>. The default value is <see langword="true"/>.</param>
		/// <param name="reloadOnChange">A value indicating whether the configuration should be reloaded if the file changes. Defaults to <see
		/// langword="false"/>.</param>
		/// <returns>The host builder instance configured to use the specified services configuration file.</returns>
		public static IHostBuilder ConfigureServicesFile(this IHostBuilder hostBuilder, string filePath, IFileProvider provider = null, bool optional = true, bool reloadOnChange = false)
		{
			return hostBuilder.ConfigureAppConfiguration(config =>
			{
				config.AddServicesConfigurationFile(filePath: filePath, provider: provider, optional: optional, reloadOnChange: reloadOnChange);
			});
		}
	}
}

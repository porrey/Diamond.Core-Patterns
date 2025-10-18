//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
		/// <returns>The <see cref="IHostBuilder"/> instance with the configuration applied.</returns>
		public static IHostBuilder ConfigureServicesFolder(this IHostBuilder hostBuilder, string folderPath)
		{
			return hostBuilder.ConfigureAppConfiguration(config =>
			{
				config.AddServicesConfigurationFolder(path: folderPath, optional: true, reloadOnChange: false);
			});
		}
	}
}

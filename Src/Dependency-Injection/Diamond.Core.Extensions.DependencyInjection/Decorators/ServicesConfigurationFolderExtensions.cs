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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods for adding the services configuration provider.
	/// </summary>
	public static class ServicesConfigurationFolderExtensions
	{
		/// <summary>
		/// Adds a configuration folder to the <see cref="IConfigurationBuilder"/> for loading service-specific
		/// configuration files.
		/// </summary>
		/// <remarks>This method allows you to include a folder containing configuration files in the
		/// configuration building process. The configuration files in the specified folder will be loaded and merged
		/// into the application's configuration.</remarks>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folder will be added.</param>
		/// <param name="folderPath">The relative or absolute path to the configuration folder.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> with the added configuration folder.</returns>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, string folderPath)
		{
			return AddServicesConfigurationFolder(builder, provider: null, folderPath: folderPath, optional: false, reloadOnChange: false);
		}

		/// <summary>
		/// Adds a folder containing service configuration files to the <see cref="IConfigurationBuilder"/>.
		/// </summary>
		/// <remarks>This method allows you to include a folder of configuration files in the application's
		/// configuration pipeline.  Use this overload if you do not need to specify a service provider or enable file change
		/// reloading.</remarks>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folder will be added.</param>
		/// <param name="folderPath">The path to the folder containing the configuration files. This path can be relative or absolute.</param>
		/// <param name="optional">A value indicating whether the configuration folder is optional.  If <see langword="true"/>, the method will not
		/// throw an exception if the folder does not exist.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> with the added configuration folder.</returns>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, string folderPath, bool optional)
		{
			return AddServicesConfigurationFolder(builder, provider: null, folderPath: folderPath, optional: optional, reloadOnChange: false);
		}

		/// <summary>
		/// Adds a configuration folder to the <see cref="IConfigurationBuilder"/> for loading service-specific configuration
		/// files.
		/// </summary>
		/// <remarks>This method allows adding a folder containing configuration files to the builder, enabling
		/// structured configuration management for services. It supports optional inclusion and automatic reloading of
		/// configuration on file changes.</remarks>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folder will be added.</param>
		/// <param name="folderPath">The relative or absolute path to the configuration folder.</param>
		/// <param name="optional"><see langword="true"/> if the configuration folder is optional; otherwise, <see langword="false"/>. If <see
		/// langword="true"/>, the method will not throw an exception if the folder is missing.</param>
		/// <param name="reloadOnChange"><see langword="true"/> to reload the configuration if files in the folder change; otherwise, <see
		/// langword="false"/>.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> with the added configuration folder.</returns>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, string folderPath, bool optional, bool reloadOnChange)
		{
			return AddServicesConfigurationFolder(builder, provider: null, folderPath: folderPath, optional: optional, reloadOnChange: reloadOnChange);
		}

		/// <summary>
		/// Adds a configuration folder to the <see cref="IConfigurationBuilder"/> for loading service-specific configuration
		/// files.
		/// </summary>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folder will be added. Cannot be <see
		/// langword="null"/>.</param>
		/// <param name="provider">The <see cref="IFileProvider"/> used to access the configuration files. Can be <see langword="null"/> to use the
		/// default file provider.</param>
		/// <param name="folderPath">The relative path to the configuration folder. Cannot be <see langword="null"/> or whitespace.</param>
		/// <param name="optional">A value indicating whether the configuration folder is optional.  If <see langword="true"/>, the configuration
		/// folder is not required to exist.</param>
		/// <param name="reloadOnChange">A value indicating whether the configuration should automatically reload if the files in the folder change.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> instance with the configuration folder added.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is <see langword="null"/> or if <paramref name="folderPath"/> is <see
		/// langword="null"/> or whitespace.</exception>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, IFileProvider provider, string folderPath, bool optional, bool reloadOnChange)
		{
			ArgumentNullException.ThrowIfNull(builder);

			if (string.IsNullOrWhiteSpace(folderPath))
			{
				throw new ArgumentNullException(nameof(folderPath));
			}

			return builder.AddServicesConfigurationFolder(s =>
			{
				s.FileProvider = provider;
				s.Path = folderPath;
				s.Optional = optional;
				s.ReloadOnChange = reloadOnChange;
				s.ResolveFileProvider();
			});
		}

		/// <summary>
		/// Adds a configuration source for a services configuration folder to the <see cref="IConfigurationBuilder"/>.
		/// </summary>
		/// <remarks>This method allows you to add a custom configuration source for managing service-specific
		/// settings. Use the <paramref name="configureSource"/> parameter to specify how the configuration source should be
		/// set up.</remarks>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration source will be added.</param>
		/// <param name="configureSource">An <see cref="Action{T}"/> delegate used to configure the <see cref="ServicesConfigurationFolderSource"/>.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> with the added configuration source.</returns>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, Action<ServicesConfigurationFolderSource> configureSource) => builder.Add(configureSource);

		/// <summary>
		/// Adds multiple configuration folders to the <see cref="IConfigurationBuilder"/> as a single source,
		/// ensuring array indices remain contiguous across all folders.
		/// </summary>
		/// <remarks>Use this overload instead of chaining multiple <see cref="AddServicesConfigurationFolder(IConfigurationBuilder, string)"/>
		/// calls when you need services from more than one folder. Because all folders are loaded by a single
		/// provider, array indices are kept contiguous and do not collide in the merged configuration.</remarks>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folders will be added.</param>
		/// <param name="folderPaths">The relative or absolute paths to the configuration folders. At least one path must be supplied.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> with the added configuration source.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is <see langword="null"/> or if <paramref name="folderPaths"/> is
		/// <see langword="null"/> or empty.</exception>
		public static IConfigurationBuilder AddServicesConfigurationFolders(this IConfigurationBuilder builder, params string[] folderPaths)
		{
			return AddServicesConfigurationFolders(builder, provider: null, folderPaths: folderPaths, optional: true, reloadOnChange: false);
		}

		/// <summary>
		/// Adds multiple configuration folders to the <see cref="IConfigurationBuilder"/> as a single source,
		/// ensuring array indices remain contiguous across all folders.
		/// </summary>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folders will be added. Cannot be <see
		/// langword="null"/>.</param>
		/// <param name="provider">The <see cref="IFileProvider"/> used to access the configuration files. Can be <see langword="null"/> to use the
		/// default file provider.</param>
		/// <param name="folderPaths">The relative or absolute paths to the configuration folders. At least one path must be supplied.</param>
		/// <param name="optional">A value indicating whether the configuration folders are optional. If <see langword="true"/>, the method will not
		/// throw an exception if a folder does not exist.</param>
		/// <param name="reloadOnChange">A value indicating whether the configuration should automatically reload if the files in a folder change.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> instance with the configuration folders added.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is <see langword="null"/> or if <paramref name="folderPaths"/> is
		/// <see langword="null"/> or empty.</exception>
		public static IConfigurationBuilder AddServicesConfigurationFolders(this IConfigurationBuilder builder, IFileProvider provider, IEnumerable<string> folderPaths, bool optional, bool reloadOnChange)
		{
			ArgumentNullException.ThrowIfNull(builder);

			string[] paths = folderPaths?.ToArray() ?? [];

			if (paths.Length == 0)
			{
				throw new ArgumentNullException(nameof(folderPaths));
			}

			return builder.AddServicesConfigurationFolder(s =>
			{
				s.FileProvider = provider;
				s.Path = paths[0];
				s.AdditionalPaths = paths.Skip(1).ToList();
				s.Optional = optional;
				s.ReloadOnChange = reloadOnChange;
				s.ResolveFileProvider();
			});
		}
	}
}

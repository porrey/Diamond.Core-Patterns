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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods for adding the services configuration provider.
	/// </summary>
	public static class ServicesConfigurationExtensions
	{
		/// <summary>
		/// Adds a configuration folder to the <see cref="IConfigurationBuilder"/> for loading service-specific
		/// configuration files.
		/// </summary>
		/// <remarks>This method allows you to include a folder containing configuration files in the
		/// configuration building process. The configuration files in the specified folder will be loaded and merged
		/// into the application's configuration.</remarks>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folder will be added.</param>
		/// <param name="path">The relative or absolute path to the configuration folder.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> with the added configuration folder.</returns>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, string path)
		{
			return AddServicesConfigurationFolder(builder, provider: null, path: path, optional: false, reloadOnChange: false);
		}

		/// <summary>
		/// Adds a folder containing service configuration files to the <see cref="IConfigurationBuilder"/>.
		/// </summary>
		/// <remarks>This method allows you to include a folder of configuration files in the application's
		/// configuration pipeline.  Use this overload if you do not need to specify a service provider or enable file change
		/// reloading.</remarks>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folder will be added.</param>
		/// <param name="path">The path to the folder containing the configuration files. This path can be relative or absolute.</param>
		/// <param name="optional">A value indicating whether the configuration folder is optional.  If <see langword="true"/>, the method will not
		/// throw an exception if the folder does not exist.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> with the added configuration folder.</returns>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, string path, bool optional)
		{
			return AddServicesConfigurationFolder(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
		}

		/// <summary>
		/// Adds a configuration folder to the <see cref="IConfigurationBuilder"/> for loading service-specific configuration
		/// files.
		/// </summary>
		/// <remarks>This method allows adding a folder containing configuration files to the builder, enabling
		/// structured configuration management for services. It supports optional inclusion and automatic reloading of
		/// configuration on file changes.</remarks>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folder will be added.</param>
		/// <param name="path">The relative or absolute path to the configuration folder.</param>
		/// <param name="optional"><see langword="true"/> if the configuration folder is optional; otherwise, <see langword="false"/>. If <see
		/// langword="true"/>, the method will not throw an exception if the folder is missing.</param>
		/// <param name="reloadOnChange"><see langword="true"/> to reload the configuration if files in the folder change; otherwise, <see
		/// langword="false"/>.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> with the added configuration folder.</returns>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
		{
			return AddServicesConfigurationFolder(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
		}

		/// <summary>
		/// Adds a configuration folder to the <see cref="IConfigurationBuilder"/> for loading service-specific configuration
		/// files.
		/// </summary>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to which the configuration folder will be added. Cannot be <see
		/// langword="null"/>.</param>
		/// <param name="provider">The <see cref="IFileProvider"/> used to access the configuration files. Can be <see langword="null"/> to use the
		/// default file provider.</param>
		/// <param name="path">The relative path to the configuration folder. Cannot be <see langword="null"/> or whitespace.</param>
		/// <param name="optional">A value indicating whether the configuration folder is optional.  If <see langword="true"/>, the configuration
		/// folder is not required to exist.</param>
		/// <param name="reloadOnChange">A value indicating whether the configuration should automatically reload if the files in the folder change.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> instance with the configuration folder added.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is <see langword="null"/> or if <paramref name="path"/> is <see
		/// langword="null"/> or whitespace.</exception>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
		{
			ArgumentNullException.ThrowIfNull(builder);

			if (string.IsNullOrWhiteSpace(path))
			{
				throw new ArgumentNullException(nameof(path));
			}

			return builder.AddServicesConfigurationFolder(s =>
			{
				s.FileProvider = provider;
				s.Path = path;
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
		/// <param name="configureSource">An <see cref="Action{T}"/> delegate used to configure the <see cref="ServicesConfigurationSource"/>.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> with the added configuration source.</returns>
		public static IConfigurationBuilder AddServicesConfigurationFolder(this IConfigurationBuilder builder, Action<ServicesConfigurationSource> configureSource) => builder.Add(configureSource);
	}
}

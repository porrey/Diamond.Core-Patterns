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
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Diamond.Core.AutoMapperExtensions
{
	/// <summary>
	/// Provides extension methods for configuring AutoMapper in .NET host and service collection builders.
	/// </summary>
	/// <remarks>These extension methods simplify the integration of AutoMapper into applications using dependency
	/// injection. They allow for automatic registration of mapping profiles and configuration of AutoMapper services
	/// within the application's dependency injection container.</remarks>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// Adds AutoMapper configuration and services to the application's dependency injection container using the specified
		/// host builder.
		/// </summary>
		/// <remarks>This method enables AutoMapper support for the application by registering required services
		/// during host building. Call this method before building the host to ensure AutoMapper is available throughout the
		/// application's lifetime.</remarks>
		/// <param name="hostBuilder">The host builder to which AutoMapper services will be added. Cannot be null.</param>
		/// <returns>The same instance of the host builder for chaining additional configuration.</returns>
		public static IHostBuilder UseAutoMapper(this IHostBuilder hostBuilder)
		{
			return hostBuilder.ConfigureServices((context, services) =>
			{
				services.UseAutoMapper();
			});
		}

		/// <summary>
		/// Adds and configures AutoMapper services using the default initialization and profiles for the application.
		/// </summary>
		/// <remarks>This method registers AutoMapper with the dependency injection container and adds all available
		/// AutoMapper profiles. It should be called during application startup before resolving AutoMapper
		/// services.</remarks>
		/// <param name="services">The service collection to which AutoMapper services will be added.</param>
		/// <returns>The same service collection instance, enabling method chaining.</returns>
		public static IServiceCollection UseAutoMapper(this IServiceCollection services)
		{
			//
			// Use the default initialization built
			// into the AutoMapper library.
			//
			services.AddAutoMapper(c =>
			{
				c.AddProfile<NullProfile>();
			});

			//
			// Replace the configuration for IConfigurationProvider
			//
			services.AddSingleton<AutoMapper.IConfigurationProvider>(sp =>
			{
				IOptions<MapperConfigurationExpression> options = sp.GetRequiredService<IOptions<MapperConfigurationExpression>>();

				//
				// Get a logger.
				//
				ILogger<IHostBuilder> logger = sp.GetRequiredService<ILogger<IHostBuilder>>();

				return new MapperConfiguration(cfg =>
				{
					IEnumerable<IProfileExpression> profileExpressions = sp.GetService<IEnumerable<IProfileExpression>>();

					foreach (IProfileExpression profileExpression in profileExpressions)
					{
						if (profileExpression is Profile profile)
						{
							logger.LogDebug("Adding Auto Mapper profile '{type}'.", profile.GetType().AssemblyQualifiedName);
							cfg.AddProfile(profile);
						}
					}
				}, sp.GetService<ILoggerFactory>());
			});

			return services;
		}
	}
}
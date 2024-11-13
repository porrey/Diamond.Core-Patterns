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
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public static class JsonHostBuilderExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
		public static IHostBuilder UseConfiguredServices(this IHostBuilder hostBuilder)
		{
			hostBuilder.AddAliases();
			hostBuilder.AddServices();
			hostBuilder.AddHostedServices();
			return hostBuilder;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
		public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
		{
			return hostBuilder.ConfigureServices(services =>
			{
				//
				// Get the service provider.
				//
				ServiceProvider sp = services.BuildServiceProvider();

				//
				// Get a logger.
				//
				ILogger<IHostBuilder> logger = sp.GetRequiredService<ILogger<IHostBuilder>>();

				//
				// Get the configuration.
				//
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
				ServiceDescriptorConfigurationDecorator.Configuration = configuration;

				//
				// Get all of the services defined in the configuration.
				//
				IList<ServiceDescriptorConfiguration> items = new List<ServiceDescriptorConfiguration>();
				configuration.Bind("services", items);

				if (items.Count() == 1)
				{
					logger.LogDebug("There was {count} service found in JSON configuration file(s).", items.Count());
				}
				else
				{
					logger.LogDebug("There were {count} services found in JSON configuration file(s).", items.Count());
				}

				//
				// Create a service descriptor for each defined service and add it to the services.
				//
				foreach (ServiceDescriptorConfiguration item in items)
				{
					ServiceDescriptor sd = item.CreateServiceDescriptor();

					if (sd != null)
					{
						string implementationType = sd.ImplementationType != null ? sd.ImplementationType.FullName : $"{nameof(DependencyFactory)}<{item.ImplementationType}>";
						logger.LogDebug("Adding service descriptor from JSON configuration for '{serviceType}' implemented by '{implementationType}' of type '{lifetime}'.", sd.ServiceType.FullName, implementationType, item.Lifetime);
						services.Add(sd);
					}
					else
					{
						logger.LogDebug("Skipping creation of service descriptor for Service Type = '{serviceType}' and ImplementationType = '{implementationType}' [Lifetime = {lifetime}].", item.ServiceType, item.ImplementationType, item.Lifetime);
					}
				}
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
		public static IHostBuilder AddAliases(this IHostBuilder hostBuilder)
		{
			return hostBuilder.ConfigureServices(services =>
			{
				//
				// Get the service provider.
				//
				ServiceProvider sp = services.BuildServiceProvider();

				//
				// Get a logger.
				//
				ILogger<IHostBuilder> logger = sp.GetRequiredService<ILogger<IHostBuilder>>();

				//
				// Get the configuration.
				//
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
				ServiceDescriptorConfigurationDecorator.Configuration = configuration;

				//
				// Get configured aliases.
				//
				IList<Alias> aliases = new List<Alias>();
				configuration.Bind("aliases", aliases);

				if (aliases.Count() == 1)
				{
					logger.LogDebug("There was {count} alias found in JSON configuration file(s).", aliases.Count());
				}
				else
				{
					logger.LogDebug("There were {count} aliases found in JSON configuration file(s).", aliases.Count());
				}

				aliases.Set();
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
		public static IHostBuilder AddHostedServices(this IHostBuilder hostBuilder)
		{
			return hostBuilder.ConfigureServices(services =>
			{
				//
				// Get the service provider.
				//
				ServiceProvider sp = services.BuildServiceProvider();

				//
				// Get a logger.
				//
				ILogger<IHostBuilder> logger = sp.GetRequiredService<ILogger<IHostBuilder>>();

				//
				// Get the configuration.
				//
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();

				//
				// Get all of the services defined in the configuration.
				//
				IList<ServiceDescriptorConfiguration> hostedItems = new List<ServiceDescriptorConfiguration>();
				configuration.Bind("hostedServices", hostedItems);

				foreach (ServiceDescriptorConfiguration hostedItem in hostedItems)
				{
					logger.LogDebug("Add hosted service '{hostedItem}'.", hostedItem.ImplementationType);
					Type implementationType = Type.GetType(hostedItem.ImplementationType, true);
					services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), implementationType));
				}
			});
		}
	}
}

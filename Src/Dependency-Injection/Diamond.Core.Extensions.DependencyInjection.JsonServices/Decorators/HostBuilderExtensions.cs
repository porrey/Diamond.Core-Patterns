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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.DependencyInjection.JsonServices
{
	/// <summary>
	/// 
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns></returns>
		public static IHostBuilder UseConfiguredServices(this IHostBuilder hostBuilder)
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
				//
				ILogger<IHostBuilder> logger = sp.GetRequiredService<ILogger<IHostBuilder>>();

				// Get the configuration.
				//
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();

				//
				// Get configured aliases.
				//
				IList<Alias> aliases = new List<Alias>();
				configuration.Bind("aliases", aliases);
				logger.LogDebug($"There were {aliases.Count()} aliases found in JSON configuration file(s).");
				aliases.Set();

				//
				// Get all of the services defined in the configuration.
				//
				IList<ServiceDescriptorConfiguration> items = new List<ServiceDescriptorConfiguration>();
				configuration.Bind("services", items);
				logger.LogDebug($"There were {items.Count()} services found in JSON configuration file(s).");

				//
				// Create a service descriptor for each defined service and add it to the services.
				//
				foreach (ServiceDescriptorConfiguration item in items)
				{
					ServiceDescriptor sd = item.CreateServiceDescriptor();
					logger.LogDebug($"Adding service descriptor from JSON configuration for '{sd.ServiceType.FullName}' implemented by '{sd.ImplementationType.FullName}' of type '{item.Lifetime}'.");
					services.Add(sd);
				}
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns></returns>
		public static IHostBuilder UseConfiguredHostedServices(this IHostBuilder hostBuilder)
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

				foreach (var hostedItem in hostedItems)
				{
					logger.LogDebug($"Add hosted service '{hostedItem.ImplementationType}'.");
					Type implementationType = Type.GetType(hostedItem.ImplementationType, true);
					services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), implementationType));
				}
			});
		}
	}
}

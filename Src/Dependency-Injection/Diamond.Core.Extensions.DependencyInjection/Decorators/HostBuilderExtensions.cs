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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder"></param>
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
				// Get the configuration.
				//
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();

				//
				// Get all of the services defined in the configuration.
				//
				IList<ServiceDescriptorConfiguration> items = new List<ServiceDescriptorConfiguration>();
				configuration.Bind("services", items);

				foreach (var item in items)
				{
					Type implementationType = Type.GetType(item.ImplementationType, true);
					Type serviceType = Type.GetType(item.ServiceType, true);

					switch (item.Lifetime)
					{
						case "Scoped":
							{
								ServiceDescriptor sd = ServiceDescriptor.Scoped(serviceType, implementationType);
								services.Add(sd);
							}
							break;
						case "Singleton":
							{
								ServiceDescriptor sd = ServiceDescriptor.Singleton(serviceType, implementationType);
								services.Add(sd);
							}
							break;
						case "Transient":
							{
								ServiceDescriptor sd = ServiceDescriptor.Transient(serviceType, implementationType);
								services.Add(sd);
							}
							break;
					}
				}
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder"></param>
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
					Type implementationType = Type.GetType(hostedItem.ImplementationType, true);
					services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), implementationType));
				}
			});
		}
	}
}

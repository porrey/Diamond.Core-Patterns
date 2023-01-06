//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.DependencyInjection.EntityFrameworkCore
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
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
		public static IHostBuilder UseConfiguredDatabaseServices(this IHostBuilder hostBuilder)
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
				IList<DatabaseDescriptorConfiguration> items = new List<DatabaseDescriptorConfiguration>();
				configuration.Bind("databases", items);

				foreach (DatabaseDescriptorConfiguration item in items)
				{
					logger.LogDebug("Add database context service '{databaseItem}'.", item.Context);
					ServiceDescriptor sd = item.CreateDatabaseDescriptor();

					if (sd != null)
					{
						logger.LogDebug("Added database service descriptor for Context = '{context}' and Connection String Key = '{connectionString}' [Lifetime = {lifetime}, Factory = {factory}].", item.Context, item.ConnectionString, item.Lifetime, item.Factory);
						services.TryAdd(sd);
					}
					else
					{
						logger.LogDebug("Skipping creation of database service descriptor for Context = '{context}' and Connection String Key = '{connectionString}' [Lifetime = {lifetime}, Factory = {factory}].", item.Context, item.ConnectionString, item.Lifetime, item.Factory);
					}
				}
			});
		}
	}
}

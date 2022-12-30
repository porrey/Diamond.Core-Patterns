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
using System;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Diamond.Core.Example
{
	/// <summary>
	/// This startup class is called by the host builder. The host build checks which
	/// interfaces are implemented and then calls the interfaces methods.
	/// </summary>
	public class ConsoleStartup : IStartupConfiguration, IStartupConfigureServices, IStartupAppConfiguration, IStartupConfigureLogging
	{
		/// <summary>
		/// 
		/// </summary>
		public IConfiguration Configuration { get; set; }

		/// <summary>
		/// Called to configure additional settings.
		/// </summary>
		/// <param name="builder"></param>
		public void ConfigureAppConfiguration(IConfigurationBuilder builder)
		{
			//
			// Build the configuration so Serilog can read from it.
			//
			IConfigurationRoot configuration = builder.Build();

			//
			// Create a logger from the configuration.
			//
			Log.Logger = new LoggerConfiguration()
					  .ReadFrom.Configuration(configuration)
					  .CreateLogger();
		}

		/// <summary>
		/// Called to add additional logging.
		/// </summary>
		/// <param name="builder"></param>
		public void ConfigureLogging(ILoggingBuilder builder)
		{
		}

		/// <summary>
		/// Called to configure additional services.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public void ConfigureServices(IServiceCollection services)
		{
			//
			// Add required services.
			//
			services.AddAutoMapper(typeof(MappingProfile));

			services.AddHttpClient(typeof(Invoice).Name, (s, c) =>
			{
				IConfiguration configuration = s.GetRequiredService<IConfiguration>();
				c.BaseAddress = new Uri(configuration[$"Settings:{typeof(Invoice).Name}:BaseUri"]);
				c.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}
	}
}

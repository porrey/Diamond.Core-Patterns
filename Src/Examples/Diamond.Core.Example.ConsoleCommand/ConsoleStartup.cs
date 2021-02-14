using System.CommandLine;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// This startup class is called by the host builder. The host build checks which
	/// interfaces are implemented and then calls the interfaces methods.
	/// </summary>
	public class ConsoleStartup : IStartupConfigureServices, IStartupAppConfiguration, IStartupConfigureLogging
	{
		/// <summary>
		/// Called to configure additional settings.
		/// </summary>
		/// <param name="builder"></param>
		public void ConfigureAppConfiguration(IConfigurationBuilder builder)
		{
		}

		/// <summary>
		/// Called to add additioonal logging.
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
			// Add the the database context.
			//
			services.AddAutoMapper(typeof(MappingProfile));
			services.AddDatabaseConfiguration();
		}
	}
}

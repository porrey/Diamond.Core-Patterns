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
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Diamond.Core.AutoMapperExtensions
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseAutoMapper(this IHostBuilder hostBuilder)
		{
			return hostBuilder.ConfigureServices((context, services) =>
			{
				services.UseAutoMapper();
			});
		}

		public static void UseAutoMapper(this IServiceCollection services)
		{
			//
			// Use the default initialization built
			// into the AutoMapper library.
			//
			services.AddAutoMapper(typeof(NullProfile));

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

				//
				// Load profiles by instance.
				//
				IEnumerable<IProfileExpression> profileExpressions = sp.GetService<IEnumerable<IProfileExpression>>();
				foreach (IProfileExpression profileExpression in profileExpressions)
				{
					if (profileExpression is Profile profile)
					{
						logger.LogDebug("Adding Auto Mapper profile '{type}'.", profile.GetType().AssemblyQualifiedName);
						options.Value.AddProfile(profile);
					}
				}

				return new MapperConfiguration(options.Value);
			});
		}
	}
}
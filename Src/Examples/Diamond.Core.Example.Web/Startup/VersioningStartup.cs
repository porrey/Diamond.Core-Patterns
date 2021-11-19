//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public static class VersioningStartup
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddMyVersioning(this IServiceCollection services)
		{
			//
			// Add version support.
			//
			services.AddApiVersioning(config =>
			{
				config.AssumeDefaultVersionWhenUnspecified = true;
				config.DefaultApiVersion = new ApiVersion(1, 0);
				config.ApiVersionReader = ApiVersionReader.Combine
				(
					new HeaderApiVersionReader("X-Version"),
					new QueryStringApiVersionReader("ver")
				);
			});

			return services;
		}
	}
}

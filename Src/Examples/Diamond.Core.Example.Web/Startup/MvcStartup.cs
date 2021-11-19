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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public static class MvcStartup
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddMyMvc(this IServiceCollection services)
		{
			services.AddMvc(config =>
			{
				//
				// These response codes are standard for all methods.
				//
				config.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status406NotAcceptable));
				config.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status500InternalServerError));
				config.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status501NotImplemented));

				//
				// Add XML support.
				//
				config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
				config.InputFormatters.Add(new XmlSerializerInputFormatter(config));

				//
				// Enable content negotiation.
				//
				config.RespectBrowserAcceptHeader = true;
				config.ReturnHttpNotAcceptable = true;

				//
				// Do not require.
				//
				config.RequireHttpsPermanent = false;
			}).SetCompatibilityVersion(CompatibilityVersion.Latest);

			//
			// Add controllers.
			//
			services.AddControllers()
					.AddNewtonsoftJson();

			return services;
		}
	}
}

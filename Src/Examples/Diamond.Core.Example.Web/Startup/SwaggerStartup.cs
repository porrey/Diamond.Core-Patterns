//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
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
using System.IO;
using Diamond.Core.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public static class SwaggerStartup
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static IApplicationBuilder UseMySwagger(this IApplicationBuilder app, IConfiguration configuration)
		{
			app.UseSwagger();

			app.UseSwaggerUI(config =>
			{
				config.SwaggerEndpoint($"swagger/{configuration["Swagger:ApiVersion"]}/swagger.json", configuration["Swagger:ApiName"]);
				config.RoutePrefix = "";
			});

			return app;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static IServiceCollection AddMySwagger(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSwaggerGen(config =>
			{
				config.SwaggerDoc(configuration["Swagger:ApiVersion"], new OpenApiInfo
				{
					Title = configuration["Swagger:ApiName"],
					Version = configuration["Swagger:DocumentationVersion"],
					Description = configuration["Swagger:ApiDescription"]
				});

				config.LoadXmlCommentFiles(new DirectoryInfo($@"{AppContext.BaseDirectory}/XmlDocs"));
				config.DescribeAllParametersInCamelCase();
				config.OperationFilter<StandardOperationFilter>();
				config.OperationFilter<UnsupportedMediaTypeFilter>();
				config.DocumentFilter<JsonPatchDocumentFilter>();
			});

			services.AddSwaggerExamplesFromAssemblyOf<JsonPatchDefaultExample>();

			return services;
		}
	}
}

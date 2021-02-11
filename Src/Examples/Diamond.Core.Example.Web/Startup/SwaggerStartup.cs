using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diamond.Core.Example {
	/// <summary>
	/// 
	/// </summary>
	public static class SwaggerStartup {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static IApplicationBuilder UseMySwagger(this IApplicationBuilder app, IConfiguration configuration) {
			app.UseSwagger();

			app.UseSwaggerUI(config => {
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
		public static IServiceCollection AddMySwagger(this IServiceCollection services, IConfiguration configuration) {
			services.AddSwaggerGen(config => {
				config.SwaggerDoc(configuration["Swagger:ApiVersion"], new OpenApiInfo {
					Title = configuration["Swagger:ApiName"],
					Version = configuration["Swagger:DocumentationVersion"],
					Description = configuration["Swagger:ApiDescription"]
				});

				SwaggerStartup.LoadXmlCommentFiles(config);
				config.DescribeAllParametersInCamelCase();

				config.OperationFilter<StandardOperationFilter>();
				config.OperationFilter<UnsupportedMediaTypeFilter>();
			});

			return services;
		}

		/// <summary>
		/// Load any XML comment files found in the folder ./XmlDocs this
		/// are to be used for Swagger documentation.
		/// </summary>
		/// <param name="config"></param>
		private static void LoadXmlCommentFiles(SwaggerGenOptions config) {
			DirectoryInfo dir = new DirectoryInfo($@"{AppContext.BaseDirectory}/XmlDocs");

			if (dir.Exists) {
				FileInfo[] files = dir.GetFiles("*.xml");

				foreach (FileInfo file in files) {
					config.IncludeXmlComments(file.FullName);
				}
			}
		}
	}
}

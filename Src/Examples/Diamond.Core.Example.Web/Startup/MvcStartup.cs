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
				// ***
				// *** These response codes are standard for all methods.
				// ***
				config.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status406NotAcceptable));
				config.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status500InternalServerError));
				config.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status501NotImplemented));

				// ***
				// *** Add XML support.
				// ***
				config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
				config.InputFormatters.Add(new XmlSerializerInputFormatter(config));

				// ***
				// *** Enable content negotiation.
				// ***
				config.RespectBrowserAcceptHeader = true;
				config.ReturnHttpNotAcceptable = true;

				// ***
				// *** Do not require.
				// ***
				config.RequireHttpsPermanent = false;
			}).SetCompatibilityVersion(CompatibilityVersion.Latest);

			// ***
			// *** Add controllers.
			// ***
			services.AddControllers()
					.AddNewtonsoftJson();

			return services;
		}
	}
}

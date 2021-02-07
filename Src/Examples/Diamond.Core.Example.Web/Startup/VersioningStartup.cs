using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class VersioningStartup
	{
		public static IServiceCollection AddMyVersioning(this IServiceCollection services)
		{
			// ***
			// *** Add version support.
			// ***
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

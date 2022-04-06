using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Diamond.Core.AutoMapperExtensions
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseAutoMapper(this IHostBuilder hostBuilder)
		{
			return hostBuilder.ConfigureServices((context, services) =>
			{
				//
				// Use the default initialization built
				// into the AutoMapper library.
				//
				services.AddAutoMapper(typeof(NullProfile));

				//
				// Replace the configuration for IConfigurationProvider
				//
				services.AddSingleton<IConfigurationProvider>(sp =>
				{
					IOptions<MapperConfigurationExpression> options = sp.GetRequiredService<IOptions<MapperConfigurationExpression>>();
					IEnumerable<IProfileExpression> profiles = sp.GetRequiredService<IEnumerable<IProfileExpression>>();

					foreach (var profile in profiles)
					{
						options.Value.AddProfile(profile.GetType());
					}

					return new MapperConfiguration(options.Value);
				});
			});
		}
	}
}
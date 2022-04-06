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
				//
				// Use the default initialization built
				// into the AutoMapper library.
				//
				services.AddAutoMapper(typeof(NullProfile));

				//
				// Get the service provider.
				//
				ServiceProvider sp = services.BuildServiceProvider();

				//
				// Get a logger.
				//
				ILogger<IHostBuilder> logger = sp.GetRequiredService<ILogger<IHostBuilder>>();

				//
				// Get the configuration.
				//
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();

				//
				// Get configured aliases.
				//
				IList<AutoMapperTypeDefinition> definitions = new List<AutoMapperTypeDefinition>();
				configuration.Bind("autoMapper", definitions);

				if (definitions.Count == 1)
				{
					logger.LogDebug("There was {count} Auto Mapper definition found in JSON configuration file(s).", definitions.Count());
				}
				else
				{
					logger.LogDebug("There were {count} Auto Mapper definitions found in JSON configuration file(s).", definitions.Count());
				}

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
					// Load profiles by type.
					//
					foreach (AutoMapperTypeDefinition definition in definitions.Where(t => t.Key.ToLower() == AutoMapperKeys.Profile))
					{
						Type type = Type.GetType(definition.TypeDefinition, true);
						logger.LogDebug("Adding Auto Mapper profile '{type}'.", type.AssemblyQualifiedName);
						options.Value.AddProfile(type);
					}

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
			});
		}
	}
}
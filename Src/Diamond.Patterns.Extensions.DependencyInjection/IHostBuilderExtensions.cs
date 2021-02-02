using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Patterns.Extensions.DependencyInjection
{
	public static class IHostBuilderExtensions
	{
		public static IHostBuilder UseDiamondDependencyPropertyInjection(this IHostBuilder hostBuilder, Action<IServiceCollection> servicesAction)
		{
			hostBuilder.UseServiceProviderFactory(new DiamondServiceProviderFactory());
			hostBuilder.ConfigureContainer<DiamondServiceProvider>(builder => servicesAction(builder.Services));
			return hostBuilder;
		}

		public static IHostBuilder UseDiamondDependencyPropertyInjection(this IHostBuilder hostBuilder)
		{
			hostBuilder.UseServiceProviderFactory(new DiamondServiceProviderFactory());
			return hostBuilder;
		}
	}
}

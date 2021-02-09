using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.InterfaceInjection {
	public static class HostBuilderExtensions {
		static readonly DiamondServiceProviderFactory _factory = new DiamondServiceProviderFactory();

		public static IHostBuilder UseDiamondDependencyInterfaceInjection(this IHostBuilder hostBuilder) {
			return hostBuilder.UseServiceProviderFactory<IServiceProvider>(new DiamondServiceProviderFactory())
					   .ConfigureContainer<IServiceCollection>((context, services) => {
						   services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceProvider>>(_factory));
						   services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceCollection>>(_factory));
					   });
		}
	}
}

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.InterfaceInjection {
	/// <summary>
	/// 
	/// </summary>
	public static class HostBuilderExtensions {
		/// <summary>
		/// 
		/// </summary>
		static readonly DiamondServiceProviderFactory _factory = new DiamondServiceProviderFactory();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder"></param>
		/// <returns></returns>
		public static IHostBuilder UseDiamondDependencyInterfaceInjection(this IHostBuilder hostBuilder) {
			return hostBuilder.UseServiceProviderFactory<IServiceProvider>(new DiamondServiceProviderFactory())
					   .ConfigureContainer<IServiceCollection>((context, services) => {
						   services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceProvider>>(_factory));
						   services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceCollection>>(_factory));
					   });
		}
	}
}

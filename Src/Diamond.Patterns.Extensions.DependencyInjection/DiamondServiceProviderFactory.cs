using System;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Patterns.Extensions.DependencyInjection
{
	public class DiamondServiceProviderFactory : IServiceProviderFactory<DiamondServiceProvider>
	{
		public DiamondServiceProvider CreateBuilder(IServiceCollection services)
		{
			return new DiamondServiceProvider(services);
		}

		public IServiceProvider CreateServiceProvider(DiamondServiceProvider containerBuilder)
		{
			return containerBuilder;
		}
	}
}

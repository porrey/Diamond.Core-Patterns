using System;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Patterns.Extensions.DependencyInjection
{
	public class DiamondServiceProviderFactory : IServiceProviderFactory<DiamondServiceProvider>, IDisposable
	{
		private IServiceCollection _services = null;
		private DiamondServiceProvider _provider = null;

		public DiamondServiceProvider CreateBuilder(IServiceCollection services)
		{
			_provider = new DiamondServiceProvider(services);
			_services = services;

			_services.AddSingleton<IServiceProviderFactory<DiamondServiceProvider>>(this);
			_services.AddSingleton<IServiceProvider>(_provider);

			return _provider;
		}

		public IServiceProvider CreateServiceProvider(DiamondServiceProvider containerBuilder)
		{
			return containerBuilder;
		}

		public void Dispose()
		{
			if (_services != null && _services is IDisposable dis1)
			{
				dis1.Dispose();
			}

			if (_provider != null && _provider is IDisposable dis2)
			{
				dis2.Dispose();
			}
		}
	}
}

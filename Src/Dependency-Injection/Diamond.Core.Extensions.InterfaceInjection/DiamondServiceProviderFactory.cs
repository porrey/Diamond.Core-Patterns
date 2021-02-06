using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Diamond.Core.Extensions.InterfaceInjection
{
	public class DiamondServiceProviderFactory : IServiceProviderFactory<IServiceProvider>, IServiceProviderFactory<IServiceCollection>, IDisposable
	{
		private DiamondServiceProvider _provider = null;

		public IServiceProvider CreateBuilder(IServiceCollection services)
		{
			_provider = new DiamondServiceProvider(services);

			_provider.Replace(ServiceDescriptor.Singleton<IServiceProvider>(_provider));
			_provider.Replace(ServiceDescriptor.Singleton<IServiceCollection>(_provider));

			return _provider;
		}

		public IServiceProvider CreateServiceProvider(IServiceProvider containerBuilder) => _provider;

		public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) => _provider;

		IServiceCollection IServiceProviderFactory<IServiceCollection>.CreateBuilder(IServiceCollection services)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			if (_provider != null && _provider is IDisposable dis)
			{
				dis.Dispose();
			}
		}	
	}
}

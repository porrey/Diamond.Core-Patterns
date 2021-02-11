using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Diamond.Core.Extensions.InterfaceInjection {
	/// <summary>
	/// 
	/// </summary>
	public class DiamondServiceProviderFactory : IServiceProviderFactory<IServiceProvider>, IServiceProviderFactory<IServiceCollection>, IDisposable {
		private DiamondServiceProvider _provider = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public IServiceProvider CreateBuilder(IServiceCollection services) {
			_provider = new DiamondServiceProvider(services);

			_provider.Replace(ServiceDescriptor.Singleton<IServiceProvider>(_provider));
			_provider.Replace(ServiceDescriptor.Singleton<IServiceCollection>(_provider));

			return _provider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="containerBuilder"></param>
		/// <returns></returns>
		public IServiceProvider CreateServiceProvider(IServiceProvider containerBuilder) => _provider;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="containerBuilder"></param>
		/// <returns></returns>
		public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) => _provider;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		IServiceCollection IServiceProviderFactory<IServiceCollection>.CreateBuilder(IServiceCollection services) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose() {
			if (_provider != null && _provider is IDisposable dis) {
				dis.Dispose();
			}
		}
	}
}

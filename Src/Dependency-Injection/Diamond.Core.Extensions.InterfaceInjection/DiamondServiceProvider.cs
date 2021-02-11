using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.InterfaceInjection {
	/// <summary>
	/// 
	/// </summary>
	public class DiamondServiceProvider : ServiceCollection, IServiceProvider, IDisposable {
		/// <summary>
		/// 
		/// </summary>
		public DiamondServiceProvider()
			: base() {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		public DiamondServiceProvider(IServiceCollection services)
			: this() {
			foreach (ServiceDescriptor sd in services) {
				this.Add(sd);
			}
		}

		private IServiceProvider _baseServiceProvider = null;
		/// <summary>
		/// 
		/// </summary>
		protected IServiceProvider BaseServiceProvider {
			get {
				if (_baseServiceProvider == null) {
					_baseServiceProvider = this.BuildServiceProvider();
				}

				return _baseServiceProvider;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public object GetService(Type serviceType) {
			object result = this.BaseServiceProvider.GetRequiredService(serviceType);

			if (result != null && result is ILoggerPublisher publisher) {
				ILoggerFactory lf = this.BaseServiceProvider.GetRequiredService<ILoggerFactory>();
				ILogger<DiamondServiceProvider> logger = lf.CreateLogger<DiamondServiceProvider>();
				logger.LogDebug($"Resolving instance of {serviceType.Name}'.");
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose() {
			if (_baseServiceProvider != null && _baseServiceProvider is IDisposable dis) {
				dis.Dispose();
			}
		}
	}
}

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.InterfaceInjection
{
	public class DiamondServiceProvider : ServiceCollection, IServiceProvider, IDisposable
	{
		public DiamondServiceProvider() 
			: base()
		{
		}

		public DiamondServiceProvider(IServiceCollection services)
			: this()
		{
			foreach (ServiceDescriptor sd in services)
			{
				this.Add(sd);
			}
		}

		private IServiceProvider _baseServiceProvider = null;
		protected IServiceProvider BaseServiceProvider
		{
			get
			{
				if (_baseServiceProvider == null)
				{
					_baseServiceProvider = this.BuildServiceProvider();
				}

				return _baseServiceProvider;
			}
		}

		public object GetService(Type serviceType)
		{
			object result = this.BaseServiceProvider.GetRequiredService(serviceType);

			if (result != null && result is ILoggerPublisher publisher)
			{
				ILoggerFactory lf = this.BaseServiceProvider.GetRequiredService<ILoggerFactory>();
				ILogger<DiamondServiceProvider> logger = lf.CreateLogger<DiamondServiceProvider>();
				logger.LogDebug($"Resolving instance of {serviceType.Name}'.");
			}

			return result;
		}

		public void Dispose()
		{
			if (_baseServiceProvider != null && _baseServiceProvider is IDisposable dis)
			{
				dis.Dispose();
			}
		}
	}
}

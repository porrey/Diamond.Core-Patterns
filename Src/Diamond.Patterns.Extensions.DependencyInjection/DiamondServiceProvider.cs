using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable DF0021

namespace Diamond.Patterns.Extensions.DependencyInjection
{
	public class DiamondServiceProvider : IServiceProvider, IDisposable
	{
		public DiamondServiceProvider(IServiceCollection services)
		{
			this.Services = services;
		}

		public IServiceCollection Services { get; protected set; }

		private IServiceProvider _serviceProvider = null;
		protected IServiceProvider ServiceProvider
		{
			get
			{
				if (_serviceProvider == null)
				{
					_serviceProvider = this.Services.BuildServiceProvider();
				}

				return _serviceProvider;
			}
		}

		public object GetService(Type serviceType)
		{
			object result = this.ServiceProvider.GetRequiredService(serviceType);

			ILoggerFactory lf = this.ServiceProvider.GetRequiredService<ILoggerFactory>();
			ILogger<DiamondServiceProvider> logger = lf.CreateLogger<DiamondServiceProvider>();
			logger.LogDebug($"Resolving instance of {serviceType.Name}'.");

			if (result != null && result is ILoggerPublisher publisher)
			{

			}

			return result;
		}

		public void Dispose()
		{
			if (_serviceProvider != null && _serviceProvider is IDisposable dis)
			{
				dis.Dispose();
			}
		}
	}
}

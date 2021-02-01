using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable DF0021

namespace Diamond.Patterns.Extensions.DependencyInjection
{
	public class DiamondServiceProvider : IServiceProvider, IDisposable
	{
		protected ServiceProvider ServiceProvider { get; set; }

		public DiamondServiceProvider(IServiceCollection services)
		{
			this.ServiceProvider = services.BuildServiceProvider();
		}

		public object GetService(Type serviceType)
		{
			object result =  this.ServiceProvider.GetRequiredService(serviceType);

			ILoggerFactory lf = this.ServiceProvider.GetRequiredService<ILoggerFactory>();
			var logger = lf.CreateLogger<DiamondServiceProvider>();
			logger.LogTrace($"Resolving instance of {serviceType.Name}'.");

			if (result != null && result is ILoggerPublisher publisher)
			{

			}

			return result;
		}

		public void Dispose()
		{
			if (this.ServiceProvider is IDisposable dis)
			{
				dis.Dispose();
			}
		}
	}
}

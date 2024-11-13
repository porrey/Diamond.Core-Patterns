//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public class DiamondServiceProvider : ServiceCollection, IServiceProvider, IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		public DiamondServiceProvider()
			: base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		public DiamondServiceProvider(IServiceCollection services)
			: this()
		{
			foreach (ServiceDescriptor sd in services)
			{
				this.Add(sd);
			}
		}

		private IServiceProvider _baseServiceProvider = null;
		/// <summary>
		/// 
		/// </summary>
		protected virtual IServiceProvider BaseServiceProvider
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public virtual object GetService(Type serviceType)
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

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			if (_baseServiceProvider != null && _baseServiceProvider is IDisposable dis)
			{
				dis.Dispose();
			}
		}
	}
}

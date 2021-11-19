//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public class DiamondServiceProviderFactory : IServiceProviderFactory<IServiceProvider>, IServiceProviderFactory<IServiceCollection>, IDisposable
	{
		private DiamondServiceProvider _provider = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public IServiceProvider CreateBuilder(IServiceCollection services)
		{
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
		IServiceCollection IServiceProviderFactory<IServiceCollection>.CreateBuilder(IServiceCollection services)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			if (_provider != null && _provider is IDisposable dis)
			{
				dis.Dispose();
			}
		}
	}
}

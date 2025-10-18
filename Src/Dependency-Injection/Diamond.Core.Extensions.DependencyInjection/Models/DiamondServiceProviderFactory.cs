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

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Provides a factory for creating and configuring <see cref="IServiceProvider"/> instances using a custom
	/// implementation.
	/// </summary>
	/// <remarks>This class implements the <see cref="IServiceProviderFactory{TContainerBuilder}"/> interface to
	/// support the creation of <see cref="IServiceProvider"/> instances from an <see cref="IServiceCollection"/> or an
	/// existing service provider. It also implements <see cref="IDisposable"/> to release resources when the factory is no
	/// longer needed.</remarks>
	public class DiamondServiceProviderFactory : IServiceProviderFactory<IServiceProvider>, IServiceProviderFactory<IServiceCollection>, IDisposable
	{
		private DiamondServiceProvider _provider = null;

		/// <summary>
		/// Creates and configures a new <see cref="IServiceProvider"/> instance using the specified service collection.
		/// </summary>
		/// <remarks>This method initializes a custom service provider and replaces the default <see
		/// cref="IServiceProvider"/> and <see cref="IServiceCollection"/> registrations with the newly created
		/// provider.</remarks>
		/// <param name="services">The <see cref="IServiceCollection"/> containing the service descriptors to be used for building the service
		/// provider.</param>
		/// <returns>An <see cref="IServiceProvider"/> instance configured with the specified services.</returns>
		public IServiceProvider CreateBuilder(IServiceCollection services)
		{
			_provider = new DiamondServiceProvider(services);
			_provider.Replace(ServiceDescriptor.Singleton<IServiceProvider>(_provider));
			_provider.Replace(ServiceDescriptor.Singleton<IServiceCollection>(_provider));

			return _provider;
		}

		/// <summary>
		/// Creates and returns an <see cref="IServiceProvider"/> instance.
		/// </summary>
		/// <param name="containerBuilder">An <see cref="IServiceProvider"/> used to build the service provider. This parameter is required but not utilized
		/// in the current implementation.</param>
		/// <returns>An <see cref="IServiceProvider"/> instance representing the service provider created.</returns>
		public IServiceProvider CreateServiceProvider(IServiceProvider containerBuilder) => _provider;

		/// <summary>
		/// Creates and returns an <see cref="IServiceProvider"/> instance based on the specified service collection.
		/// </summary>
		/// <param name="containerBuilder">The <see cref="IServiceCollection"/> used to configure the service provider.</param>
		/// <returns>An <see cref="IServiceProvider"/> instance that resolves services configured in the provided service collection.</returns>
		public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) => _provider;

		/// <summary>
		/// Creates a builder for configuring and building an <see cref="IServiceProvider"/> using the specified <see
		/// cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/> to configure. This parameter cannot be <see langword="null"/>.</param>
		/// <returns>An <see cref="IServiceCollection"/> that can be used to configure services for the <see cref="IServiceProvider"/>.</returns>
		/// <exception cref="NotImplementedException"></exception>
		IServiceCollection IServiceProviderFactory<IServiceCollection>.CreateBuilder(IServiceCollection services)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Releases the resources used by the current instance of the class.
		/// </summary>
		/// <remarks>This method disposes of any resources held by the instance, including  disposing the underlying
		/// provider if it implements <see cref="IDisposable"/>. After calling this method, the instance should not be
		/// used.</remarks>
		public void Dispose()
		{
			if (_provider != null && _provider is IDisposable dis)
			{
				dis.Dispose();
			}
		}
	}
}

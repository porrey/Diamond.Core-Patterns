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
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Provides extension methods for configuring an <see cref="IHostBuilder"/> to use the Diamond dependency injection
	/// framework.
	/// </summary>
	/// <remarks>These extensions allow the <see cref="IHostBuilder"/> to be configured with the <see
	/// cref="DiamondServiceProviderFactory"/>, enabling advanced dependency injection capabilities such as interface-based
	/// injection and service replacement.</remarks>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// A static, read-only instance of the <see cref="DiamondServiceProviderFactory"/> class.
		/// </summary>
		/// <remarks>This instance is used to provide a shared, thread-safe factory for creating service
		/// providers.</remarks>
		static readonly DiamondServiceProviderFactory _factory = new DiamondServiceProviderFactory();

		/// <summary>
		/// Configures the specified <see cref="IHostBuilder"/> to use the Diamond dependency injection model.
		/// </summary>
		/// <remarks>This method replaces the default service provider factory with a custom implementation that uses
		/// the Diamond dependency injection model. It also configures the container to replace the default service provider
		/// factory registrations with the custom factory.</remarks>
		/// <param name="hostBuilder">The <see cref="IHostBuilder"/> to configure.</param>
		/// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
		public static IHostBuilder UseDiamondDependencyInterfaceInjection(this IHostBuilder hostBuilder)
		{
			return hostBuilder.UseServiceProviderFactory<IServiceProvider>(new DiamondServiceProviderFactory())
					   .ConfigureContainer<IServiceCollection>((context, services) =>
					   {
						   services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceProvider>>(_factory));
						   services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceCollection>>(_factory));
					   });
		}
	}
}

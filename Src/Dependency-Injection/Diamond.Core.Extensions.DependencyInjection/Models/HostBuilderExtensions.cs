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
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		static readonly DiamondServiceProviderFactory _factory = new DiamondServiceProviderFactory();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
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

//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.System.TemporaryFolder
{
	/// <summary>
	/// <see cref="IHostBuilder" /> builder extensions.
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// Add the temporary folder factory to the IServiceCollection.
		/// </summary>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining</returns>
		public static IHostBuilder UseTemporaryFolderFactory(this IHostBuilder hostBuilder)
		{
			hostBuilder.ConfigureServices((c, s) =>
			{
				s.AddScoped<ITemporaryFolderFactory, TemporaryFolderFactory>();
			});

			return hostBuilder;
		}
	}
}

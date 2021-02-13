//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.Configuration.Services
{
	/// <summary>
	/// 
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostBuilder"></param>
		/// /// <param name="folderPath"></param>
		/// <returns></returns>
		public static IHostBuilder ConfigureServicesFolder(this IHostBuilder hostBuilder, string folderPath)
		{
			return hostBuilder.ConfigureAppConfiguration(config =>
			{
				config.AddServicesConfigurationFolder(path: folderPath, optional: true, reloadOnChange: false);
			});
		}
	}
}

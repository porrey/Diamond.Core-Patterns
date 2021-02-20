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
using System.Threading.Tasks;
using Diamond.Core.CommandLine;
using Diamond.Core.Extensions.DependencyInjection;
using Diamond.Core.Extensions.DependencyInjection.EntityFrameworkCore;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Diamond.Core.Clonable.Newtonsoft;
using Diamond.Core.System.TemporaryFolder;

//
// See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0
// for details on host based applications.
//

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class Program
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		static Task<int> Main(string[] args) => Host.CreateDefaultBuilder(args)
							.AddRootCommand("Sample Application", args)
							.UseSerilog((c, l) =>
							{
								l.ReadFrom.Configuration(c.Configuration);
							})
							.ConfigureServicesFolder("Services")
							.UseConfiguredServices()
							.UseConfiguredDatabaseServices()
							.UseStartup<ConsoleStartup>()
							.UseObjectCloning()
							.UseTemporaryFolderFactory()
							.UseConsoleLifetime()
							.Build()
							.RunWithExitCodeAsync();
	}
}
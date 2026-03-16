//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example.BasicConsole
{
	class Program
	{
		static Task Main(string[] args) => Host.CreateDefaultBuilder(args)
				.AddRootCommand("Sample Application", args)
				.UseStartup<ConsoleStartup>()
				.UseConsoleLifetime()
				.ConfigureServicesFolder("Services", reloadOnChange: true, optional: false)
				.UseConfiguredServices()
				.Build()
				.RunWithExitCodeAsync();
	}
}
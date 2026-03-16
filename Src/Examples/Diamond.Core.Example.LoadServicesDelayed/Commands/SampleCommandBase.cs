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
using System;
using System.Threading.Tasks;
using Diamond.Core.CommandLine.Model;
using Diamond.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.LoadServicesDelayed
{
	/// <summary>
	/// Represents a base command for executing operations on a sample model.	
	/// </summary>
	/// <remarks>This class provides foundational functionality for commands targeting the SampleModel type. It is
	/// intended to be extended by specific command implementations. The command is initialized with a logger and
	/// predefined command metadata.</remarks>
	public class SampleCommandBase : ModelCommand<SampleModel>
	{
		/// <summary>
		/// Initializes a new instance of the SampleCommandBase class with the specified logger.
		/// </summary>
		/// <param name="logger">The logger used to record diagnostic and operational messages for the command.</param>
		public SampleCommandBase(ILogger<SampleCommandBase> logger, IServiceProvider serviceProvider)
			: base(logger, "test", "Run a test command.")
		{
			this.ServiceProvider = serviceProvider;
		}

		protected IServiceProvider ServiceProvider { get; }

		protected override Task<int> OnHandleCommand(SampleModel item)
		{
			int returnValue = 0;

			IHost builder = (new HostBuilder())
								.ConfigureServices((c, s) => { })
								.UseServiceProviderFactory(c => new MyFactory(this.ServiceProvider))
								.ConfigureServicesFile("Services/Example1.json")
								.ConfigureServicesFile("Configuration/Example2.json")
								.UseConfiguredServices()
								.Build();

			ISample sample2 = builder.Services.GetKeyedService<ISample>("Sample2");
			Console.WriteLine(sample2.Name);

			return Task.FromResult(returnValue);
		}
	}

	public class MyFactory : IServiceProviderFactory<IServiceCollection>
	{
		public MyFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		protected IServiceProvider ServiceProvider { get; }

		public IServiceCollection CreateBuilder(IServiceCollection services)
		{
			return services;
		}

		public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
		{
			return containerBuilder.BuildServiceProvider();
		}
	}
}

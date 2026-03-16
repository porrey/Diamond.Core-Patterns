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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.Example.LoadServices;
using Diamond.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Diamond.Core.Example
{
	/// <summary>
	/// Provides the entry point for the application. Initializes services from configuration and outputs the sample
	/// service name.
	/// </summary>
	/// <remarks>This class is intended to be used as the main entry point for the application. It reads service
	/// configuration from a JSON file, registers services, and builds a service provider. The application writes the name
	/// of the sample service to the console. The entry point is asynchronous to support modern .NET application
	/// patterns.</remarks>
	public class Program
	{
		/// <summary>
		/// The entry point for the application. Initializes services from configuration and outputs the sample service name.
		/// </summary>
		/// <remarks>This method reads service configuration from a JSON file, registers services, and builds a
		/// service provider. The application writes the name of the sample service to the console. The method is asynchronous
		/// to support modern .NET application patterns.</remarks>
		/// <param name="_">An array of command-line arguments. Not used by this application.</param>
		/// <returns>A task that represents the completion of the application. The task result is 0 to indicate successful execution.</returns>
		static Task<int> Main(string[] _)
		{
			int returnValue = 0;

			//
			// Read the configuration file.
			//
			string json = File.ReadAllText("./Services/Example.json");

			//
			// Deserialize the descriptors.
			//
			ServiceDescriptorConfigurationFile descriptors = JsonConvert.DeserializeObject<ServiceDescriptorConfigurationFile>(json);

			//
			// Create a service collection.
			//
			IServiceCollection services = new ServiceCollection();

			//
			// Set the alias list for the service descriptor configuration decorator.
			//
			ServiceDescriptorConfigurationDecorator.Set(descriptors.Aliases);

			//
			// Create service descriptors and add them to the service collection.
			//
			foreach (ServiceDescriptorConfiguration item in descriptors.Services)
			{
				//
				// Create a service descriptor from the configuration item.
				//
				ServiceDescriptor sd = item.CreateServiceDescriptor();

				//
				// If a service descriptor was created, add it to the service collection.
				//
				if (sd != null)
				{
					//
					// Add the service descriptor to the service collection.
					//
					services.Add(sd);
				}
			}

			//
			// Build the service provider.
			//
			DefaultServiceProviderFactory defaultServiceProviderFactory = new();
			IServiceProvider serviceProvider = defaultServiceProviderFactory.CreateServiceProvider(services);

			//
			// Get the ISample instance from the service provider.
			//
			ISample sample1 = serviceProvider.GetService<ISample>();
			Console.WriteLine(sample1.Name);

			ISample sample2 = serviceProvider.GetKeyedService<ISample>("Sample2");
			Console.WriteLine(sample2.Name);

			return Task.FromResult(returnValue);
		}
	}
}
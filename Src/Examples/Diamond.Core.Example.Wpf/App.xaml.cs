using Diamond.Core.Extensions.DependencyInjection;
using Diamond.Core.Wpf;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example.Wpf
{
	public partial class App : HostedApplication
	{
		protected override IHostBuilder OnConfigureHost(IHostBuilder hostBuilder)
		{
			//
			// Load services from JSON files found in the ./Services folder.
			//
			return hostBuilder.ConfigureServicesFolder("Services");
		}
	}
}

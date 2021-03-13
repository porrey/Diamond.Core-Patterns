using Diamond.Core.Extensions.DependencyInjection;
using Diamond.Core.Wpf;
using Microsoft.Extensions.Configuration;

namespace Diamond.Core.Example.Wpf
{
	public partial class App : HostedApplication
	{
		protected override void OnConfigureHostConfiguration(IConfigurationBuilder configurationBuilder)
		{
			//
			// Load services from JSON files found in the ./Services folder.
			//
			configurationBuilder.AddServicesConfigurationFolder("Services");
		}
	}
}

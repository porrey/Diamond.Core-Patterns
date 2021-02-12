using Diamond.Core.Extensions.Configuration.Services;
using Diamond.Core.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example {
	/// <summary>
	/// 
	/// </summary>
	public class Program {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServicesFolder("./Config")
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseStartup<Startup>();
				})
				.UseConfiguredServices()
				.UseConfiguredHostedServices()
				.Build()
				.Run();
	}
}
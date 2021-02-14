using Diamond.Core.Extensions.Configuration.JsonServices;
using Diamond.Core.Extensions.DependencyInjection.JsonServices;
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
				.ConfigureServicesFolder("./Services")
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseStartup<Startup>();
				})
				.UseConfiguredServices()
				.UseConfiguredHostedServices()
				.Build()
				.Run();
	}
}
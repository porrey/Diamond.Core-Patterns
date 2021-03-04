using Diamond.Core.Extensions.DependencyInjection;
using Diamond.Core.Extensions.DependencyInjection.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

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
		static void Main(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureServicesFolder("Services")
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.UseConfiguredServices()
				.UseConfiguredDatabaseServices()
				.Build()
				.Run();
	}
}
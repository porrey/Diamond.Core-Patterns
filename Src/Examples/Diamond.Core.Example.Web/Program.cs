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
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseStartup<Startup>();
				})
			.Build()
			.Run();
	}
}

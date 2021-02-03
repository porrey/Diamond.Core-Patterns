using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example
{
	public class Program
	{
		public static IHostBuilder CreateWebBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});

		public static void Main(string[] args)
		{
			CreateWebBuilder(args).Build().Run();
		}
	}
}

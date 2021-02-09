using System.Threading.Tasks;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example
{
	class Program
	{
		static Task<int> Main(string[] args) => Host.CreateDefaultBuilder(args)
				.UseStartup<ConsoleStartup>()
				.RunCommandAsync();
	}
}

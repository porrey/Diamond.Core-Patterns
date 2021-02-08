using System.Threading.Tasks;
using Diamond.Core.ConsoleCommands;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example.ConsoleCommand
{
	class Program
	{
		static async Task Main(string[] args) => await ConsoleHost.CreateRootCommand("Sample Application", args)
				.UseDiamondCoreHost<ConsoleStartup>(args)
				.RunConsoleAsync();
	}
}

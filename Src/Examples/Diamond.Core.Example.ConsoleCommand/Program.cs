using System.Threading.Tasks;
using Diamond.Core.ConsoleCommands;
using Diamond.Core.Extensions.Hosting;

namespace Diamond.Core.Example.ConsoleCommand
{
	class Program
	{
		static async Task Main(string[] args) => await ConsoleHost.CreateRootCommand("Sample Application", args)
				.UseDiamondCoreHost<ConsoleStartup>(args)
				.RunCommandAsync();
	}
}
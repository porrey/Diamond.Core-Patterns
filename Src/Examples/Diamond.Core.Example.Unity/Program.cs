using System.Threading.Tasks;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace Diamond.Core.Example {
	class Program {
		static Task<int> Main(string[] args) => Host.CreateDefaultBuilder(args)
				.UseUnityServiceProvider()
				.UseStartup<ConsoleStartup, IUnityContainer>()
				.RunCommandAsync();
	}
}

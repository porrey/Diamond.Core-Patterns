using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example
{
	class Program
	{
		public static IHostBuilder CreateConsoleBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseConsoleLifetime()
				.ConfigureServices(services => Program.ConfigureServices(services));

		private static void ConfigureServices(IServiceCollection services)
		{
			// ***
			// *** Add the Diamond Patterns dependencies needed for the examples.
			// ***
			services.AddWorkFlowExampleDependencies();
			services.AddSpecificationExampleDependencies();
			services.AddRulesExampleDependencies();
			services.AddDecoratorExampleDependencies();
			services.AddUnitOfWorkExampleDependencies();
			services.AddRepositoryExampleDependencies();

			// ***
			// *** Add the example application services.
			// ***
			services.AddHostedService<WorkFlowExampleHostedService>();
			services.AddHostedService<SpecificationExampleHostedService>();
			services.AddHostedService<RulesExampleHostedService>();
			services.AddHostedService<DecoratorExampleHostedService>();
			services.AddHostedService<UnitOfWorkExampleHostedService>();
			services.AddHostedService<RepositoryExampleHostedService>();
		}

		static Task Main(string[] args)
		{
			// ***
			// *** Run as console application
			// ***
			return CreateConsoleBuilder(args).Build().RunAsync();
		}
	}
}

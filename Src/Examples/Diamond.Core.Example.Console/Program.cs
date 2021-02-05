using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example
{
	class Program
	{
		static Task Main(string[] args) =>
			(Host.CreateDefaultBuilder(args)
				.UseConsoleLifetime()
				.ConfigureServices(services => Program.ConfigureServices(services)))
				.Build()
				.RunAsync();

		private static void ConfigureServices(IServiceCollection services)
		{
			// ***
			// *** Add the Diamond Core dependencies needed for the examples.
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
	}
}

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example
{
	class Program
	{
		static Task Main(string[] args) =>
			(Host.CreateDefaultBuilder(args)
				.ConfigureServices((services) => Program.ConfigureMyServices(services)))
				.RunConsoleAsync();

		private static void ConfigureMyServices(IServiceCollection services)
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

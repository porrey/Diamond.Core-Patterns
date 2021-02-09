using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class ConsoleStartup : IStartupConfigureServices
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public void ConfigureServices(IServiceCollection services)
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

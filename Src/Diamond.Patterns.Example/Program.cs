using System.Threading.Tasks;
using Diamond.Patterns.Extensions.DependencyInjection;
using Diamond.Patterns.WorkFlow;
using Diamond.Patterns.WorkFlow.Decorators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Patterns.Example
{
	class Program
	{
		public static IHostBuilder CreateConsoleBuilder(string[] args) =>
							Host.CreateDefaultBuilder(args)
								.UseDiamondDependencyInjection()
								.ConfigureServices(services =>
								{
									Program.ConfigureServices(services);
								});

		private static IServiceCollection ConfigureServices(IServiceCollection services)
		{
			// ***
			// *** Add the core application service.
			// ***
			services.AddHostedService<WorkFlowExampleHostedService>();

			// ***
			// *** Add the default work-flow factories.
			// ***
			services.UseDiamondWorkFlow();

			// ***
			// *** Create a linear work flow called "Group1".
			// ***
			services.AddTransient<IWorkFlowManager>(sp => new LinearWorkFlowManager(sp.GetRequiredService<IWorkFlowItemFactory>(), "Group1"));

			// ***
			// *** Create a linear work flow called "Group2".
			// ***
			services.AddTransient<IWorkFlowManager>(sp => new LinearWorkFlowManager(sp.GetRequiredService<IWorkFlowItemFactory>(), "Group2"));

			// ***
			// *** Add work flow items for "Group1".
			// ***
			for (int i = 0; i < 10; i++)
			{
				services.AddTransient<IWorkFlowItem>(sp => new SampleWorkFlowItem("Group1", i + 1, $"Step {i + 1}")
				{
					Logger = sp.GetRequiredService<ILogger<SampleWorkFlowItem>>()
				});
			}

			return services;
		}

		static Task Main(string[] args)
		{
			return CreateConsoleBuilder(args).Build().RunAsync();
		}
	}
}

using System.Threading.Tasks;
using Diamond.Patterns.WorkFlow;
using Diamond.Patterns.WorkFlow.Decorators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Patterns.Example
{
	class Program
	{
		public static IHostBuilder CreateConsoleBuilder(string[] args) =>
							Host.CreateDefaultBuilder(args)
								.UseConsoleLifetime()
								.ConfigureServices(services => Program.ConfigureServices(services))
								//.UseDiamondDependencyPropertyInjection(services => Program.ConfigureServices(services))
								;

		private static void ConfigureServices(IServiceCollection services)
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
			services.AddWorkFlowManager<LinearWorkFlowManager>("Group1")
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 1)
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 2)
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 3)
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 4)
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 5)
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 6)
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 7)
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 8)
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 9)
					.AddWorkFlowItem<SampleWorkFlowItem>("Group1", 10);

			// ***
			// *** Create a conditional work flow called "Group2".
			// ***
			services.AddWorkFlowManager<ConditionalWorkFlowManager>("Group2")
					.AddWorkFlowItem<CreateTemporaryFolderStep>("Group2", 1)
					.AddWorkFlowItem<DeleteTemporaryFolderStep>("Group2", 2);
		}

		static Task Main(string[] args)
		{
			return CreateConsoleBuilder(args).Build().RunAsync();
		}
	}
}

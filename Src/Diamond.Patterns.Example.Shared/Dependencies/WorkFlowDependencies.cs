using Diamond.Patterns.WorkFlow;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Patterns.Example
{
	public static class WorkFlowDependencies
	{
		public static IServiceCollection AddWorkFlowDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the default work-flow factories.
			// ***
			services.UseDiamondWorkFlow();

			// ***
			// *** Create a linear work flow.
			// ***
			services.AddTransient<IWorkFlowManager, SampleWorkFlowManager>();
			services.AddTransient<IWorkFlowItem, SampleWorkStep1>();
			services.AddTransient<IWorkFlowItem, SampleWorkStep2>();
			services.AddTransient<IWorkFlowItem, SampleWorkStep3>();
			services.AddTransient<IWorkFlowItem, SampleWorkStep4>();
			services.AddTransient<IWorkFlowItem, SampleWorkStep5>();

			return services;
		}
	}
}

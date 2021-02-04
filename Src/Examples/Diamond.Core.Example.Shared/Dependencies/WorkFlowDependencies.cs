using Diamond.Core.WorkFlow;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class WorkFlowDependencies
	{
		public static IServiceCollection AddWorkFlowExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the default dependencies.
			// ***
			services.UseDiamondWorkFlowPattern();

			// ***
			// *** Add the sample work flow manager and work flow steps.
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

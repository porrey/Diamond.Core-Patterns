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
			services.AddScoped<IWorkFlowManagerFactory, WorkFlowManagerFactory>()
					.AddScoped<IWorkFlowItemFactory, WorkFlowItemFactory>()
					.AddScoped<IWorkFlowManager, SampleWorkFlowManager>()
					.AddScoped<IWorkFlowItem, SampleWorkStep1>()
					.AddScoped<IWorkFlowItem, SampleWorkStep2>()
					.AddScoped<IWorkFlowItem, SampleWorkStep3>()
					.AddScoped<IWorkFlowItem, SampleWorkStep4>()
					.AddScoped<IWorkFlowItem, SampleWorkStep5>();

			return services;
		}
	}
}

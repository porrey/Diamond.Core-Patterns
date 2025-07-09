using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Extends the IServiceCollection object.
	/// </summary>
	public static class ServiceCollectionDecorator
	{
		/// <summary>
		/// Adds the necesarys registrations to the service collection.
		/// </summary>
		/// <param name="services"></param>
		public static IServiceCollection UseWorkflowFactory(this IServiceCollection services)
		{
			services.AddTransient<IWorkflowManagerFactory, WorkflowManagerFactory>();
			services.AddTransient<IWorkflowItemFactory, WorkflowItemFactory>();
			return services;
		}
	}
}

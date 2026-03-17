using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Provides extension methods for configuring workflow-related services in an IServiceCollection.
	/// </summary>
	/// <remarks>This class enables registration of workflow factories and managers for dependency injection. Use
	/// its methods to add workflow functionality to your application's service container.</remarks>
	public static class ServiceCollectionDecorator
	{
		/// <summary>
		/// Registers workflow-related factories with the dependency injection container.
		/// </summary>
		/// <remarks>This method adds transient implementations for workflow manager and workflow item factories. Use
		/// this extension when configuring services to enable workflow functionality in your application.</remarks>
		/// <param name="services">The service collection to which workflow factory services will be added. Cannot be null.</param>
		/// <returns>The updated service collection with workflow factory services registered.</returns>
		public static IServiceCollection UseWorkflowFactory(this IServiceCollection services)
		{
			services.AddTransient<IWorkflowManagerFactory, WorkflowManagerFactory>();
			services.AddTransient<IWorkflowItemFactory, WorkflowItemFactory>();
			return services;
		}
	}
}

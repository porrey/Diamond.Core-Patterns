using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Patterns.WorkFlow.Decorators
{
	/// <summary>
	/// 
	/// </summary>
	public static class ServicesExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection UseDiamondWorkFlow(this IServiceCollection services)
		{
			// ***
			// *** Add the WorkFlowManagerFactory.
			// ***
			services.AddSingleton<IWorkFlowManagerFactory, WorkFlowManagerFactory>();

			// ***
			// *** Add the WorkFlowItemFactory.
			// ***
			services.AddSingleton<IWorkFlowItemFactory, WorkFlowItemFactory>();

			return services;
		}
	}
}

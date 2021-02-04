using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.WorkFlow
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
		public static IServiceCollection UseDiamondWorkFlowPattern(this IServiceCollection services)
		{
			// ***
			// *** Add the WorkFlowManagerFactory.
			// ***
			services.AddSingleton<IWorkFlowManagerFactory>(sp =>
			{
				WorkFlowManagerFactory item = new WorkFlowManagerFactory(sp)
				{
					Logger = sp.GetRequiredService<ILogger<WorkFlowManagerFactory>>()
				};

				return item;
			});

			// ***
			// *** Add the WorkFlowItemFactory.
			// ***
			services.AddSingleton<IWorkFlowItemFactory>(sp=>
			{
				WorkFlowItemFactory item = new WorkFlowItemFactory(sp)
				{
					Logger = sp.GetRequiredService<ILogger<WorkFlowItemFactory>>()
				};

				return item;
			});

			return services;
		}
	}
}

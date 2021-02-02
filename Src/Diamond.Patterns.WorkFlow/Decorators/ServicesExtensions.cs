using Diamond.Patterns.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TWorkFlowManager"></typeparam>
		/// <param name="services"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		public static IServiceCollection AddWorkFlowManager<TWorkFlowManager>(this IServiceCollection services, string group)
			where TWorkFlowManager : IWorkFlowManager, new()
		{
			services.AddTransient<IWorkFlowManager>(sp =>
			{
				IWorkFlowManager item = new TWorkFlowManager()
				{
					Group = group,
					WorkFlowItemFactory = sp.GetRequiredService<IWorkFlowItemFactory>()
				};

				if (item is ILoggerPublisher<TWorkFlowManager> publisher)
				{
					publisher.Logger = sp.GetRequiredService<ILogger<TWorkFlowManager>>();
				}

				return item;
			});

			return services;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TWorkFlowItem"></typeparam>
		/// <param name="services"></param>
		/// <param name="group"></param>
		/// <param name="ordinal"></param>
		/// <returns></returns>
		public static IServiceCollection AddWorkFlowItem<TWorkFlowItem>(this IServiceCollection services, string group, int ordinal)
			where TWorkFlowItem : IWorkFlowItem, new()
		{
			services.AddTransient<IWorkFlowItem>(sp =>
			{
				IWorkFlowItem item = new TWorkFlowItem()
				{
					Group = group,
					Ordinal = ordinal
				};

				if (item is ILoggerPublisher<TWorkFlowItem> publisher)
				{
					publisher.Logger = sp.GetRequiredService<ILogger<TWorkFlowItem>>();
				}

				return item;
			});

			return services;
		}
	}
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.UnitOfWork
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
		public static IServiceCollection UseDiamondUnitOfWork(this IServiceCollection services)
		{
			// ***
			// *** Add the WorkFlowManagerFactory.
			// ***
			services.AddSingleton<IUnitOfWorkFactory>(sp =>
			{
				UnitOfWorkFactory item = new UnitOfWorkFactory(sp)
				{
					Logger = sp.GetRequiredService<ILogger<UnitOfWorkFactory>>()
				};

				return item;
			});

			return services;
		}
	}
}

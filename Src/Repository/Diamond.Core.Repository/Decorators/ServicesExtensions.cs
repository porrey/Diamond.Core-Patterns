using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Repository {
	/// <summary>
	/// 
	/// </summary>
	public static class ServicesExtensions {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection UseDiamondRepositoryPattern(this IServiceCollection services) {
			//
			// Add the WorkFlowManagerFactory.
			//
			services.AddSingleton<IRepositoryFactory>(sp => {
				RepositoryFactory item = new RepositoryFactory(sp) {
					Logger = sp.GetRequiredService<ILogger<RepositoryFactory>>()
				};

				return item;
			});

			return services;
		}
	}
}

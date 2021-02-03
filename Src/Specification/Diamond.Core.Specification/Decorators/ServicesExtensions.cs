using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Specification
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
		public static IServiceCollection UseDiamondSpecification(this IServiceCollection services)
		{
			// ***
			// *** Add the WorkFlowManagerFactory.
			// ***
			services.AddSingleton<ISpecificationFactory>(sp =>
			{
				SpecificationFactory item = new SpecificationFactory(sp)
				{
					Logger = sp.GetRequiredService<ILogger<SpecificationFactory>>()
				};

				return item;
			});

			return services;
		}
	}
}

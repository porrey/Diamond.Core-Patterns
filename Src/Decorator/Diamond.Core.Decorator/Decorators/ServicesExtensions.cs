using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Decorator
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
		public static IServiceCollection UseDiamondDecorator(this IServiceCollection services)
		{
			// ***
			// *** Add the WorkFlowManagerFactory.
			// ***
			services.AddSingleton<IDecoratorFactory>(sp =>
			{
				DecoratorFactory item = new DecoratorFactory(sp)
				{
					Logger = sp.GetRequiredService<ILogger<DecoratorFactory>>()
				};

				return item;
			});

			return services;
		}
	}
}

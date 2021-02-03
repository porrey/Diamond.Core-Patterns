using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Rules
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
		public static IServiceCollection UseDiamondRules(this IServiceCollection services)
		{
			// ***
			// *** Add the WorkFlowManagerFactory.
			// ***
			services.AddSingleton<IRulesFactory>(sp =>
			{
				RulesFactory item = new RulesFactory(sp)
				{
					Logger = sp.GetRequiredService<ILogger<RulesFactory>>()
				};

				return item;
			});

			return services;
		}
	}
}

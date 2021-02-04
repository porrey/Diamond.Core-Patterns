using Diamond.Core.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class RulesDependencies
	{
		public static IServiceCollection AddRulesExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the default dependencies.
			// ***
			services.UseDiamondRulesPattern();

			// ***
			// *** Add the rules to validate the shipment model. 
			// ***
			services.AddTransient<IRule, WeightRule>();
			services.AddTransient<IRule, PickupAddressRule>();
			services.AddTransient<IRule, DeliveryAddressRule>();
			services.AddTransient<IRule, PalletCountRule>();

			return services;
		}
	}
}

using Diamond.Patterns.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Patterns.Example
{
	public static class RulesDependencies
	{
		public static IServiceCollection AddRulesExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the default dependencies.
			// ***
			services.UseDiamondRules();

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

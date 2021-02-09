using Diamond.Core.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example {
	public static class RulesDependencies {
		public static IServiceCollection AddRulesExampleDependencies(this IServiceCollection services) {
			//
			// Add the rules to validate the shipment model. 
			//
			services.AddTransient<IRulesFactory, RulesFactory>()
					.AddTransient<IRule, WeightRule>()
					.AddTransient<IRule, PickupAddressRule>()
					.AddTransient<IRule, DeliveryAddressRule>()
					.AddTransient<IRule, PalletCountRule>();

			return services;
		}
	}
}

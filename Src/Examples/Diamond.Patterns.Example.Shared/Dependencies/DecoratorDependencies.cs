using Diamond.Patterns.Decorator;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Patterns.Example
{
	public static class DecoratorDependencies
	{
		public static IServiceCollection AddDecoratorExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the default dependencies.
			// ***
			services.UseDiamondDecorator();

			// ***
			// *** Add the rules to validate the shipment model. 
			// ***
			services.AddTransient<IDecorator, BookTransactionDecorator>();

			return services;
		}
	}
}

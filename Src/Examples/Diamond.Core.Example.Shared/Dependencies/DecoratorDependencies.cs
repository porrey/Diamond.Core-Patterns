using Diamond.Core.Decorator;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class DecoratorDependencies
	{
		public static IServiceCollection AddDecoratorExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the rules to validate the shipment model. 
			// ***
			services.AddScoped<IDecoratorFactory, DecoratorFactory>()
					.AddScoped<IDecorator, BookTransactionDecorator>();

			return services;
		}
	}
}

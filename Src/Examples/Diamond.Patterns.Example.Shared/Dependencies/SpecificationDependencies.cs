using Diamond.Patterns.Specification;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Patterns.Example
{
	public static class SpecificationDependencies
	{
		public static IServiceCollection AddSpecificationExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the default dependencies.
			// ***
			services.UseDiamondSpecification();

			// ***
			// *** Add the specification to qualify the widgets.
			// ***
			services.AddTransient<ISpecification, WidgetQualificationSpecification>();

			return services;
		}
	}
}

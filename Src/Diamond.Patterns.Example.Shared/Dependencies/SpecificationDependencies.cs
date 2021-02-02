using Diamond.Patterns.Specification;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Patterns.Example
{
	public static class SpecificationDependencies
	{
		public static IServiceCollection AddSpecificationDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the default work-flow factories.
			// ***
			services.UseDiamondSpecification();

			// ***
			// *** Create a linear work flow named WellKnown.Specification.QualifyWidget.
			// ***
			services.AddTransient<ISpecification, WidgetQualificationSpecification>();

			return services;
		}
	}
}

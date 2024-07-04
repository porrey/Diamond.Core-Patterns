using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Specification
{
	/// <summary>
	/// Extends the IServiceCollection object.
	/// </summary>
	public static class ServiceCollectionDecorator
	{
		/// <summary>
		/// Adds the necesarys registrations to the service collection.
		/// </summary>
		/// <param name="services"></param>
		public static void UseSpecificationFactory(this IServiceCollection services)
		{
			services.AddTransient<ISpecificationFactory, SpecificationFactory>();
		}
	}
}

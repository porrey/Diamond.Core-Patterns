using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Decorator
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
		public static IServiceCollection UseDecoratorFactory(this IServiceCollection services)
		{
			services.AddTransient<IDecoratorFactory, DecoratorFactory>();
			return services;
		}
	}
}

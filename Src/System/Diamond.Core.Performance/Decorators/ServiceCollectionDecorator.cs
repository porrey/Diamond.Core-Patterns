using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Performance
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
		public static IServiceCollection UseMeasureAction(this IServiceCollection services)
		{
			services.AddTransient<IMeasureAction, MeasureAction>();
			return services;
		}
	}
}

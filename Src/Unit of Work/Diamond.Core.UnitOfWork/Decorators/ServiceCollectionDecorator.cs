using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.UnitOfWork
{
	/// <summary>
	/// Extends the IServiceCollection object.
	/// </summary>
	public static class ServiceCollectionDecorator
	{
		/// <summary>
		/// Adds the necessary registrations to the service collection.
		/// </summary>
		/// <param name="services"></param>
		public static IServiceCollection UseUnitOfWorkFactory(this IServiceCollection services)
		{
			services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
			return services;
		}
	}
}

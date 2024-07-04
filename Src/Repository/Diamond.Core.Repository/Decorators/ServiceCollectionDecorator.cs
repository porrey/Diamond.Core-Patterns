using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Repository
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
		public static void UseRepositoryFactory(this IServiceCollection services)
		{
			services.AddTransient<IRepositoryFactory, RepositoryFactory>();
		}
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Repository.EntityFrameworkCore
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
		public static IServiceCollection UseEntityRepository<TInterface, TEntity, TContext>(this IServiceCollection services)
			where TEntity : class, TInterface, new()
			where TInterface : IEntity
			where TContext : DbContext, IRepositoryContext
		{
			services.AddTransient<IEntityFactory<TInterface>, EntityFactory<TInterface, TEntity>>();
			services.AddTransient<IRepository<TInterface>, EntityFrameworkRepository<TInterface, TEntity, TContext>>();
			return services;
		}
	}
}

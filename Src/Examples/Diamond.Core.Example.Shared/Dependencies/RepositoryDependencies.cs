using Diamond.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class RepositoryDependencies
	{
		public static IServiceCollection AddRepositoryExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the entity factory and repository to the container.
			// ***
			services.AddScoped< IRepositoryFactory, RepositoryFactory>()
					.AddScoped<IEntityFactory<IInvoice>, InvoiceEntityFactory>()
					.AddScoped<IRepository<IInvoice>, InvoiceRepository>();

			services.AddDbContext<ErpContext>((sp, options) =>
			{
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
				options.UseInMemoryDatabase(configuration["ErpDatabase:InMemory"]);
				//options.UseNpgsql(configuration["ErpDatabase:PostgreSQL"]);
				//options.UseSqlite(configuration["ErpDatabase:SQLite"]);
				//options.UseSqlServer(configuration["ErpDatabase:SqlServer"]);
			});

			return services;
		}
	}
}

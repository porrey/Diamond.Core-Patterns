using Diamond.Core.AspNet.DoAction;
using Diamond.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class DiamondCoreStartup
	{
		public static IServiceCollection AddMyDiamondCore(this IServiceCollection services, IConfiguration configuration)
		{
			// ***
			// *** Add the data storage services.
			// ***
			services.AddScoped<IDoActionFactory, DoActionFactory>()
					.AddScoped<IRepositoryFactory, RepositoryFactory>()
					.AddScoped<IEntityFactory<IInvoice>, InvoiceEntityFactory>()
					.AddScoped<IRepository<IInvoice>, InvoiceRepository>()
					.AddDbContext<ErpContext>(options =>
					{
						options.UseInMemoryDatabase(configuration["ErpDatabase:InMemory"]);
						//options.UseNpgsql(configuration["ErpDatabase:PostgreSQL"]);
						//options.UseSqlite(configuration["ErpDatabase:SQLite"]);
						//options.UseSqlServer(configuration["ErpDatabase:SqlServer"]);
					});

			// ***
			// *** Add the hosted service to populate the sample database.
			// ***
			services.AddHostedService<RepositoryExampleHostedService>();

			return services;
		}
	}
}

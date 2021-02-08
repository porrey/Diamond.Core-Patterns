using Diamond.Core.AspNet.DoAction;
using Diamond.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public static class DiamondCoreStartup
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddMyDiamondCore(this IServiceCollection services)
		{
			// ***
			// *** Add the data storage services.
			// ***
			services.AddScoped<IDoActionFactory, DoActionFactory>()
					.AddScoped<IRepositoryFactory, RepositoryFactory>()
					.AddScoped<IEntityFactory<IInvoice>, InvoiceEntityFactory>()
					.AddScoped<IRepository<IInvoice>, InvoiceRepository>()
					.AddDbContext<ErpContext>((sp, options) =>
					{
						IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
						options.UseInMemoryDatabase(configuration["ErpDatabase:InMemory"]);
						//options.UseNpgsql(configuration["ErpDatabase:PostgreSQL"]);
						//options.UseSqlite(configuration["ErpDatabase:SQLite"]);
						//options.UseSqlServer(configuration["ErpDatabase:SqlServer"]);
					})
					.AddScoped<IDoAction, GetAllInvoicesAsyncAction>()
					.AddScoped<IDoAction, CreateInvoiceAsyncAction>()
					.AddScoped<IDoAction, GetInvoiceAsyncAction>()
					.AddScoped<IDoAction, UpdateInvoiceAsyncAction>()
					.AddScoped<IDoAction, DeleteInvoiceAsyncAction>()
					.AddScoped<IDoAction, MarkInvoicePaidAsyncAction>();

			// ***
			// *** Add the hosted service to populate the sample database.
			// ***
			services.AddHostedService<RepositoryExampleHostedService>();

			return services;
		}
	}
}

using Diamond.Core.Repository;
using Diamond.Core.Repository.EntityFrameworkCore.InMemory;
using Diamond.Core.Repository.EntityFrameworkCore.Sqlite;
using Diamond.Core.Repository.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class RepositoryDependencies
	{
		public static IServiceCollection AddRepositoryExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the default dependencies.
			// ***
			services.UseDiamondRepositoryPattern();

			// ***
			// *** Add items to read and write data to a SQLite database.
			// ***
			services.AddSingleton<IEntityFactory<IInvoice>, InvoiceEntityFactory>();
			services.AddTransient<IRepository<IInvoice>, InvoiceRepository>();

			// ***
			// *** Uncomment only one line below to select the data storage. Note that SQL server will require
			// *** migrations to be run first.
			// ***
			services.AddSingleton<IContextFactory<ErpContext>, InMemoryContextFactory<ErpContext>>();
			//services.AddSingleton<IContextFactory<ErpContext>, SqliteContextFactory<ErpContext>>();
			//services.AddSingleton<IContextFactory<ErpContext>, SqlServerContextFactory<ErpContext>>();

			return services;
		}
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class DatabaseServicesExtensions
	{
		public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
		{
			//
			// Add the data storage services.
			//
			services.AddDbContext<ErpContext>((sp, options) =>
			{
				//
				// Create a DatabaseOptions object to hold the application settings.
				//
				DatabaseOptions databaseOptions = new DatabaseOptions();

				//
				// Get the configuration and bind the object to the data.
				//
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
				configuration.Bind(DatabaseOptions.Key, databaseOptions);

				//
				// Select the active database configuration.
				//
				switch (databaseOptions.ActiveDatabase)
				{
					case ActiveDatabase.InMemory:
						options.UseInMemoryDatabase(databaseOptions.InMemory);
						break;
					case ActiveDatabase.SqlServer:
						options.UseSqlServer(databaseOptions.SqlServer);
						break;
					case ActiveDatabase.PostgreSQL:
						options.UseNpgsql(databaseOptions.PostgreSQL);
						break;
					case ActiveDatabase.SQLite:
						options.UseSqlite(databaseOptions.SQLite);
						break;
				}
			});

			return services;
		}
	}
}

//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
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

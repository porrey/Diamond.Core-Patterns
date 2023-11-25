﻿//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class RepositoryExampleHostedService : IHostedService
	{
		private readonly ILogger<RepositoryExampleHostedService> _logger = null;
		private readonly IHostApplicationLifetime _appLifetime = null;
		private readonly IConfiguration _configuration = null;
		private readonly IServiceScopeFactory _serviceScopeFactory = null;

		private int _exitCode = 0;

		public RepositoryExampleHostedService(ILogger<RepositoryExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
		{
			_logger = logger;
			_appLifetime = appLifetime;
			_configuration = configuration;
			_serviceScopeFactory = serviceScopeFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Starting {nameof(RepositoryExampleHostedService)} service.");

			//
			// Since this hosted service runs as a singleton we need
			// a scope to get access to scoped services.
			//
			using (IServiceScope scope = _serviceScopeFactory.CreateScope())
			{
				//
				// Get the IRepositoryFactory service.
				//
				IRepositoryFactory repositoryFactory = scope.ServiceProvider.GetService<IRepositoryFactory>();

				//
				// Get a writable repository for IInvoice.
				//
				IWritableRepository<IInvoice> repository = await repositoryFactory.GetWritableAsync<IInvoice>();

				//
				// Ensure the database is created.
				//
				using (IRepositoryContext db = await repository.AsQueryable().GetContextAsync())
				{
					if (await db.EnsureCreatedAsync())
					{
						if (!(await repository.AsReadOnly().GetAllAsync()).Any())
						{
							//
							// Create 100 new items.
							//
							Random rnd = new Random();

							for (int i = 0; i < 100; i++)
							{
								//
								// Create a new empty model.
								//
								IInvoice model = await repository.ModelFactory.CreateAsync();

								//
								// Assign properties.
								//
								model.Total = rnd.Next(10, 10000);
								model.Number = $"INV{rnd.Next(1, 2000000):0000000}";
								model.Description = $"Shipment Invoice {i}";

								//
								// Add the new item to the database.
								//
								(int affected, IInvoice invoice) = await repository.AddAsync(model);

								if (affected > 0)
								{
									_logger.LogInformation($"Successfully create invoice with ID = {invoice.Id} [{i}].");
								}
								else
								{
									_logger.LogError($"Failed to create new invoice [{i}].");
								}
							}
						}
					}

					//
					// Query the database and retrieve all of the invoices.
					//
					IEnumerable<IInvoice> items = await repository.AsReadOnly().GetAllAsync();
					_logger.LogInformation($"There are {items.Count()} invoices in the database.");
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Exiting service {nameof(RepositoryExampleHostedService)} with return code: {_exitCode}");

			//
			// Exit code.
			//
			Environment.ExitCode = _exitCode;
			return Task.CompletedTask;
		}
	}
}

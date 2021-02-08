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
			_logger.LogInformation("Starting application.");

			// ***
			// *** Since this hosted service runs as a singletin we need
			// *** a scope to get access to scped services.
			// ***
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				// ***
				// *** Get the IRepositoryFactory service.
				// ***
				IRepositoryFactory repositoryFactory = scope.ServiceProvider.GetService<IRepositoryFactory>();

				// ***
				// *** Get a writable repostiroy for IInvoice.
				// ***
				IWritableRepository<IInvoice> repository = await repositoryFactory.GetWritableAsync<IInvoice>();

				// ***
				// *** Ensure the database is created.
				// ***
				using (IRepositoryContext db = await repository.GetContextAsync())
				{
					if (await db.EnsureCreated())
					{
						if (!(await repository.GetAllAsync()).Any())
						{
							// ***
							// *** Create 100 new items.
							// ***
							Random rnd = new Random();

							for (int i = 0; i < 100; i++)
							{
								// ***
								// *** Create a new empty model.
								// ***
								IInvoice model = await repository.ModelFactory.CreateAsync();

								// ***
								// *** Assign properties.
								// ***
								model.Total = rnd.Next(10, 10000);
								model.Number = $"INV{rnd.Next(1, 2000000):0000000}";
								model.Description = $"Invoice {i}.";

								// ***
								// *** Add the new item to the database.
								// ***
								(bool result, IInvoice invoice) = await repository.AddAsync(model);

								if (result)
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

					// ***
					// *** Query the database and retrieve all of the invoices.
					// ***
					IEnumerable<IInvoice> items = await repository.GetAllAsync();
					_logger.LogInformation($"There are {items.Count()} invoices in the database.");
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Exiting with return code: {_exitCode}");

			// ***
			// *** Exit code.
			// ***
			Environment.ExitCode = _exitCode;
			return Task.CompletedTask;
		}
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example {
	public class UnitOfWorkExampleHostedService : IHostedService {
		private readonly ILogger<UnitOfWorkExampleHostedService> _logger = null;
		private readonly IHostApplicationLifetime _appLifetime = null;
		private readonly IConfiguration _configuration = null;
		private readonly IUnitOfWorkFactory _unitOfWorkFactory = null;

		private int _exitCode = 0;

		public UnitOfWorkExampleHostedService(ILogger<UnitOfWorkExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, IUnitOfWorkFactory unitOfWorkFactory) {
			_logger = logger;
			_appLifetime = appLifetime;
			_configuration = configuration;
			_unitOfWorkFactory = unitOfWorkFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken) {
			_logger.LogInformation("Starting application.");

			//*
			//* Create a person.
			//*
			IPerson person = new Person() {
				Id = 1,
				FullName = "John Doe"
			};

			//*
			//* Get a Unit of Work to create an appointment in the storage system.
			//*
			IUnitOfWork<(bool, IAppointment), (IPerson, DateTime)> uow = await _unitOfWorkFactory.GetAsync<(bool, IAppointment), (IPerson, DateTime)>(WellKnown.UnitOfWork.CreateAppointment);

			//*
			//* Create the appointment in the storage system.
			//*
			(bool result, IAppointment appointment) = await uow.CommitAsync((person, DateTime.Now.AddDays(5)));

			if (result) {
				_logger.LogInformation($"The appointment was successfully created. The appointmnet ID is {appointment.Id}.");
			}
			else {
				_logger.LogError("Failed to create appointment.");
			}
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			_logger.LogDebug($"Exiting with return code: {_exitCode}");

			//
			// Exit code.
			//
			Environment.ExitCode = _exitCode;
			return Task.CompletedTask;
		}
	}
}

using System;
using System.Threading.Tasks;
using Diamond.Core.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class CreateAppointmentUnitOfWork : IUnitOfWork<(bool, IAppointment), (IPerson Person, DateTime AppointmentTime)>
	{
		public CreateAppointmentUnitOfWork(ILogger<CreateAppointmentUnitOfWork> logger)
		{
			this.Logger = logger;
		}

		public string Key => WellKnown.UnitOfWork.CreateAppointment;
		protected ILogger<CreateAppointmentUnitOfWork> Logger { get; set; }

		public Task<(bool, IAppointment)> CommitAsync((IPerson Person, DateTime AppointmentTime) item)
		{
			(bool result, IAppointment appointment) = (false, null);

			this.Logger.LogInformation($"Creating appointmnet for '{item.Person.FullName}'.");

			appointment = new Appointment()
			{
				Id = 101,
				Person = item.Person,
				AppointmentTime = item.AppointmentTime
			};

			result = true;

			return Task.FromResult((result, appointment));
		}
	}
}

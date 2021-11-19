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

		public string Name => WellKnown.UnitOfWork.CreateAppointment;
		protected ILogger<CreateAppointmentUnitOfWork> Logger { get; set; }

		public Task<(bool, IAppointment)> CommitAsync((IPerson Person, DateTime AppointmentTime) item)
		{
			(bool result, IAppointment appointment) = (false, null);

			this.Logger.LogInformation($"Creating appointment for '{item.Person.FullName}'.");

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

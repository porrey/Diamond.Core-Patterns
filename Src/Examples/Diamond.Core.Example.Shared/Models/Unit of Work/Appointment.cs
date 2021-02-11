using System;

namespace Diamond.Core.Example {
	public class Appointment : IAppointment {
		public int Id { get; set; }
		public IPerson Person { get; set; }
		public DateTime AppointmentTime { get; set; }
	}
}

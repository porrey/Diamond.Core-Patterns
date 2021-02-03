using System;

namespace Diamond.Patterns.Example
{
	public interface IAppointment
	{
		int Id { get; set; }
		IPerson Person { get; set; }
		DateTime AppointmentTime { get; set; }
	}
}

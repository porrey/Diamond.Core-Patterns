using System;
using Diamond.Core.Repository;

namespace Diamond.Core.Example.BasicConsole
{
	public interface IEmployeeEntity : IEntity<int>
	{
		string FirstName { get; set; }
		string LastName { get; set; }
		string JobTitle { get; set; }
		decimal Compensation { get; set; }
		DateTime StartDate { get; set; }
		DateTime? LastPromtion { get; set; }
		bool Active { get; set; }
		bool RecentWarnings { get; set; }
	}
}
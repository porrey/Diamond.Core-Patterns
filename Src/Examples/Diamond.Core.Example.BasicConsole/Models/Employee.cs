using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Diamond.Core.Repository;

namespace Diamond.Core.Example.BasicConsole
{
	[Table("Employee", Schema = "HR")]
	public class EmployeeEntity : Entity<int>, IEmployeeEntity
	{
		[Column("EmployeeId", Order = 0)]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required]
		public new int Id { get; set; }

		[MaxLength(50)]
		public string FirstName { get; set; }

		[MaxLength(50)]
		public string LastName { get; set; }

		[MaxLength(100)]
		public string JobTitle { get; set; }

		public DateTime StartDate { get; set; }

		public decimal Compensation { get; set; }

		public bool Active { get; set; }

		public DateTime? LastPromtion { get; set; }

		public bool RecentWarnings { get; set; }

		public override string ToString()
		{
			return $"{this.LastName}, {this.FirstName}, Title = '{this.JobTitle}' [ID = {this.Id}, Active = {this.Active}]";
		}
	}
}

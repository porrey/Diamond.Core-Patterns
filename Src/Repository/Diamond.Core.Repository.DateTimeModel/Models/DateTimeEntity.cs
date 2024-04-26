using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Diamond.Core.Repository.DateTimeModel
{
	/// <summary>
	/// This is a base class for all OtmIntegration database models. Using this
	/// base class will provide a consistency for all tables.
	/// </summary>
	public abstract class DateTimeEntity : Entity, IDateTimeEntity
	{
		/// <summary>
		/// Get/sets the data and time this entity model was created and first stored
		/// in the database/repository. This date and time stamp should never
		/// be changed after a value has been set.
		/// </summary>
		[Required]
		[Column(Order = 9995)]
		[JsonIgnore]
		public DateTime CreatedDateTime { get; set; } = DateTime.MinValue;

		/// <summary>
		/// Gets/sets the name of the user who created this entity model.This field  value
		/// should never be changed after a value has been set.
		/// </summary>
		[StringLength(50)]
		[Required]
		[Column(Order = 9996)]
		[JsonIgnore]
		public string CreatedBy { get; set; }

		/// <summary>
		/// Gets/sets the date and time this entity model was last updated in the
		/// database/repository.
		/// </summary>
		[Required]
		[Column(Order = 9997)]
		[JsonIgnore]
		public DateTime ModifiedDateTime { get; set; } = DateTime.MinValue;

		/// <summary>
		/// Gets/sets the name of the user who last modified this entity model in the
		/// database/repository.
		/// </summary>
		[StringLength(50)]
		[Required]
		[Column(Order = 9998)]
		[JsonIgnore]
		public string ModifiedBy { get; set; }
	}
}

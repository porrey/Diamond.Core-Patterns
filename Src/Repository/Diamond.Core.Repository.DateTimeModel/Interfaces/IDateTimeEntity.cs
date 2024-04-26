using System;
using Diamond.Core.Repository;

namespace Diamond.Core.Repository.DateTimeModel
{
	/// <summary>
	/// Defines an interface for all entity models to inherit
	/// from.
	/// </summary>
	public interface IDateTimeEntity : IEntity
	{
		/// <summary>
		/// Get/sets the data and time this entity model was created and first stored
		/// in the database/repository. This date and time stamp should never
		/// be changed after a value has been set.
		/// </summary>
		DateTime CreatedDateTime { get; set; }

		/// <summary>
		/// Gets/sets the name of the user who created this entity model.This field  value
		/// should never be changed after a value has been set.
		/// </summary>
		string CreatedBy { get; set; }

		/// <summary>
		/// Gets/sets the date and time this entity model was last updated in the
		/// database/repository.
		/// </summary>
		DateTime ModifiedDateTime { get; set; }

		/// <summary>
		/// Gets/sets the name of the user who last modified this entity model in the
		/// database/repository.
		/// </summary>
		string ModifiedBy { get; set; }
	}
}
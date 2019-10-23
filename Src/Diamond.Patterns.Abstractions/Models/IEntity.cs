using System;

namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Base entity class allowing generic classes for any
	/// interface defined as an "Entity".
	/// </summary>
	public interface IEntity : ICloneable
	{
	}

	/// <summary>
	/// Base entity class with a "ID" defined as type T. Each
	/// entity (or model) will defined it's own ID type based
	/// on the Mail.dat specification.
	/// </summary>
	/// <typeparam name="T">The type of ID for this entity.</typeparam>
	public interface IEntity<T> : IEntity
	{
		/// <summary>
		/// Get/sets or unique ID for this item.
		/// </summary>
		T Id { get; set; }
	}
}

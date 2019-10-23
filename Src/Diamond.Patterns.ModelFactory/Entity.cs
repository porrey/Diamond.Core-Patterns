using Diamond.Patterns.Abstractions;
using Diamond.Patterns.System;

namespace Diamond.Patterns.ModelFactory
{
	public abstract class Entity : Cloneable, IEntity
	{
	}

	public class Entity<T> : Entity, IEntity<T>
	{
		public virtual T Id { get; set; }
	}
}

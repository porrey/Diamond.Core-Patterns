using System;

namespace Diamond.Patterns.Abstractions
{
	public interface IDatabaseStrategy<TContext>
	{
		object GetInitializer(object modelBuilder, EventHandler<TContext> onSeed);
	}
}

using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Decorator
{
	public class DecoratorNotFoundException<TItem, TResult> : DiamondPatternsException
	{
		public DecoratorNotFoundException()
			: base($"A decorator of type 'IDecorator<{typeof(TItem).Name}, {typeof(TResult).Name}>' has not been configured.")
		{
		}

		public DecoratorNotFoundException(string name)
			: base($"A decorator of type 'IDecorator<{typeof(TItem).Name}, {typeof(TResult).Name}>' named '{name}' has not been configured.")
		{
		}
	}
}

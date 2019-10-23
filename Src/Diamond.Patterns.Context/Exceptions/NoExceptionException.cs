using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	public class NoExceptionException : DiamondPatternsException
	{
		public NoExceptionException()
			: base($"An exception object was not found in the context properties.")
		{
		}
	}
}

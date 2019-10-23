using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	public class NoExitCodeException : DiamondPatternsException
	{
		public NoExitCodeException()
			: base($"An exit code was not found in the context properties.")
		{
		}
	}
}

using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Repository.EntityFramework
{
	public class InvalidContextException : DiamondPatternsException
	{
		public InvalidContextException()
			: base($"The IDatabaseContext instance provided is invalid.")
		{
		}
	}
}

using System;

namespace Diamond.Patterns.Repository
{
	public class RepositoryNotInitializedException : RepositoryException
	{
		public RepositoryNotInitializedException(Type type)
			: base($"The repository for Type '{type.Name}' failed to initialize.")
		{
		}
	}
}

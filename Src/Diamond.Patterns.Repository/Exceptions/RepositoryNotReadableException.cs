using System;

namespace Diamond.Patterns.Repository
{
	public class RepositoryNotReadableException : RepositoryException
	{
		public RepositoryNotReadableException(Type t)
			: base($"The repository {t.Name} does not implement IReadOnlyRepository.")
		{
		}
	}
}

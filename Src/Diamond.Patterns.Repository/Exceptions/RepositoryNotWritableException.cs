using System;

namespace Diamond.Patterns.Repository
{
	public class RepositoryNotWritableException : RepositoryException
	{
		public RepositoryNotWritableException(Type t)
			: base($"The repository {t.Name} does not implement IWritableRepository.")
		{
		}
	}
}

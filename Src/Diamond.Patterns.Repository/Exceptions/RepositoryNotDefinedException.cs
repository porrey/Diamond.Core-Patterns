using System;

namespace Diamond.Patterns.Repository
{
	public class RepositoryNotDefinedException : RepositoryException
	{
		public RepositoryNotDefinedException(Type t)
			: base($"A repository for Type {t.Name} has not been configured.")
		{
		}
	}
}

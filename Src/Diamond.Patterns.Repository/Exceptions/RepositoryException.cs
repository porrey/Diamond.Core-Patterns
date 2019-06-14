using System;
using System.Runtime.Serialization;

namespace Diamond.Patterns.Repository
{
	public abstract class RepositoryException : Exception
	{
		public RepositoryException()
			: base()
		{
		}

		public RepositoryException(string message)
				: base(message)
		{
		}

		protected RepositoryException(SerializationInfo info, StreamingContext context)
				: base(info, context)
		{
		}

		public RepositoryException(string message, Exception innerException) :
				base(message, innerException)
		{
		}
	}
}

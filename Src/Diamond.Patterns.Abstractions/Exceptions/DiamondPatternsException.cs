using System;
using System.Runtime.Serialization;

namespace Diamond.Patterns.Abstractions
{
	public abstract class DiamondPatternsException : Exception
	{
		public DiamondPatternsException()
			: base()
		{
		}

		public DiamondPatternsException(string message)
				: base(message)
		{
		}

		protected DiamondPatternsException(SerializationInfo info, StreamingContext context)
				: base(info, context)
		{
		}

		public DiamondPatternsException(string message, Exception innerException) :
				base(message, innerException)
		{
		}
	}
}

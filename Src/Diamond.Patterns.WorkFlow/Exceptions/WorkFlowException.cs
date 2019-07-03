using System;
using System.Runtime.Serialization;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	public abstract class WorkFlowException : DiamondPatternsException
	{
		public WorkFlowException()
			: base()
		{
		}

		public WorkFlowException(string message)
				: base(message)
		{
		}

		protected WorkFlowException(SerializationInfo info, StreamingContext context)
				: base(info, context)
		{
		}

		public WorkFlowException(string message, Exception innerException) :
				base(message, innerException)
		{
		}
	}
}

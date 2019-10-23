using System;

namespace Diamond.Patterns.WorkFlow
{
	/// <summary>
	/// Generic error for a missing context property.
	/// </summary>
	public class WorkFlowFailureException : WorkFlowException
	{
		public WorkFlowFailureException(Exception innerException, int step)
			: base($"The work flow failure at step {step}. See the inner exception for details.", innerException)
		{
		}
	}
}

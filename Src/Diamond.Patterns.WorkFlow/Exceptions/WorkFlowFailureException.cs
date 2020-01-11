using System;

namespace Diamond.Patterns.WorkFlow
{
	/// <summary>
	/// Generic error for a missing context property.
	/// </summary>
	public class WorkFlowFailureException : WorkFlowException
	{
		public WorkFlowFailureException(Exception innerException,string stepName, int stepNumber)
			: base($"The work flow failure at the step '{stepName}' [{stepNumber}]. See the inner exception for details.", innerException)
		{
		}
	}
}

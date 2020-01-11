namespace Diamond.Patterns.WorkFlow
{
	/// <summary>
	/// Generic error for a missing context property.
	/// </summary>
	public class UnknownFailureException : WorkFlowException
	{
		public UnknownFailureException(string stepName, int stepNumber)
			: base($"The step '{stepName}' [{stepNumber}] failed for an unknown reason.")
		{
		}
	}
}

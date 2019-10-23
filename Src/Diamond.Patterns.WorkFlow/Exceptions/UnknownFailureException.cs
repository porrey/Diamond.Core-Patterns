namespace Diamond.Patterns.WorkFlow
{
	/// <summary>
	/// Generic error for a missing context property.
	/// </summary>
	public class UnknownFailureException : WorkFlowException
	{
		public UnknownFailureException(string stepName)
			: base($"The step '{stepName}' failed for an unknown reason.")
		{
		}
	}
}

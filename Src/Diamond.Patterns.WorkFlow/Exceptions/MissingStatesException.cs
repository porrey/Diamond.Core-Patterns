namespace Diamond.Patterns.WorkFlow
{
	/// <summary>
	/// Generic error for a missing context property.
	/// </summary>
	public class MissingStepsException : WorkFlowException
	{
		public MissingStepsException(string group)
			: base($"No Work-FLow steps with group '{group}' were found.")
		{
		}
	}
}

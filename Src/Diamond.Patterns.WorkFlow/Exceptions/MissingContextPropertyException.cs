namespace Diamond.Patterns.WorkFlow
{
	/// <summary>
	/// Generic error for a missing context property.
	/// </summary>
	public class MissingContextPropertyException : WorkFlowException
	{
		public MissingContextPropertyException(string key)
			: base($"The context dictionary does not have a property named '{key}'.")
		{
		}
	}
}

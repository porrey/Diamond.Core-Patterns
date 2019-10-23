using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.StateDictionary
{
	/// <summary>
	/// Generic error for a missing context property.
	/// </summary>
	public class MissingContextPropertyException : DiamondPatternsException
	{
		public MissingContextPropertyException(string key)
			: base($"The context dictionary does not have a property named '{key}'.")
		{
		}
	}
}

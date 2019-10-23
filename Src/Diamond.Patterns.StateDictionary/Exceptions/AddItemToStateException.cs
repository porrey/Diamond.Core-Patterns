using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.StateDictionary
{
	public class AddItemToStateException : DiamondPatternsException
	{
		public AddItemToStateException(string propertyName)
			: base($"Failed to add the item '{propertyName}' to the context state.")
		{
		}
	}
}

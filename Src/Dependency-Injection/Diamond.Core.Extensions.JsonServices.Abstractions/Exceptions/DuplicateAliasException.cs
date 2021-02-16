namespace Diamond.Core.Extensions.JsonServices
{
	/// <summary>
	/// 
	/// </summary>
	public class DuplicateAliasException : DiamondJsonServicesException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		public DuplicateAliasException(string key)
			: base($"A duplicate definition for type alias with the key '{key}' has been encountered.")
		{
		}
	}
}

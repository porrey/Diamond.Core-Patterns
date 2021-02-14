namespace Diamond.Core.Extensions.DependencyInjection.JsonServices
{
	/// <summary>
	/// 
	/// </summary>
	public class AliasNotFoundException : DiamondJsonServicesException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		public AliasNotFoundException(string key)
			: base($"A definition for type alias with the key '{key}' was not found.")
		{
		}
	}
}

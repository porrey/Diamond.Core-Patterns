namespace Diamond.Core.CommandLine
{
	/// <summary>
	/// 
	/// </summary>
	public class NoCommandsConfiguredException : DiamondCommandLineException
	{
		/// <summary>
		/// 
		/// </summary>
		public NoCommandsConfiguredException()
			: base($"No ICommand objects were found in the service container.")
		{
		}
	}
}

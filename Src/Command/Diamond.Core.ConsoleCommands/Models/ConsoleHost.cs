namespace Diamond.Core.ConsoleCommands
{
	/// <summary>
	/// 
	/// </summary>
	public static class ConsoleHost
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static RootCommandService CreateRootCommand(string name, string[] args)
		{
			return new RootCommandService(name, args);
		}
	}
}

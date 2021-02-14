using System.CommandLine;

namespace Diamond.Core.CommandLine
{
	/// <summary>
	/// Distinguishes a root command from a regular command.
	/// </summary>
	public interface IRootCommand : ICommand
	{
		/// <summary>
		/// 
		/// </summary>
		string[] Args { get; set; }
	}
}

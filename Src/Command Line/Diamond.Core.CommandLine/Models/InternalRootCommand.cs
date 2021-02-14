using System.CommandLine;

namespace Diamond.Core.CommandLine
{
	internal class InternalRootCommand : RootCommand, IRootCommand
	{
		public InternalRootCommand(string description, string[] args) 
			: base(description)
		{
			this.Args = args;
		}

		public string[] Args { get; set; }
	}
}

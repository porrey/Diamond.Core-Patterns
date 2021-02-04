using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.ConsoleCommand
{
	public class HelloCommand : Command
	{
		public HelloCommand(ILogger<HelloCommand> logger) : base("hello", "Responds with a hello greeting.")
		{
			this.Logger = logger;

			Option<int> option = new Option<int>("--name", "Full name of person to greet.")
			{
				IsRequired = true
			};

			this.AddOption(option);

			this.Handler = CommandHandler.Create<HelloProperties>(async (p) =>
			{
				return await this.OnHandleCommand(p);
			});
		}

		protected ILogger<HelloCommand> Logger { get; set; }

		protected Task<int> OnHandleCommand(HelloProperties properties)
		{
			this.Logger.LogInformation($"Hello '{properties.Name}.");
			return Task.FromResult(0);
		}
	}
}

using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Example.ConsoleCommand
{
	/// <summary>
	/// 
	/// </summary>
	public class EchoCommand : Command
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public EchoCommand(ILogger<EchoCommand> logger)
			: base("echo", "Responds by echoing back the phrase.")
		{
			this.Logger = logger;

			Option<string> option = new Option<string>($"--{nameof(EchoProperties.Phrase).ToLower()}", "The phrase to echo back.")
			{
				IsRequired = true
			};

			this.AddOption(option);

			this.Handler = CommandHandler.Create<EchoProperties>(async (p) =>
			{
				return await this.OnHandleCommand(p);
			});
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<EchoCommand> Logger { get; set; } = new NullLogger<EchoCommand>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="properties"></param>
		/// <returns></returns>
		protected Task<int> OnHandleCommand(EchoProperties properties)
		{
			this.Logger.LogInformation($"You said: '{properties.Phrase}'.");
			return Task.FromResult(0);
		}
	}
}

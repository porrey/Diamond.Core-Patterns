using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Example.ConsoleCommand {
	/// <summary>
	/// 
	/// </summary>
	public class HelloCommand : Command {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public HelloCommand(ILogger<HelloCommand> logger)
			: base("hello", "Responds with a hello greeting.") {
			this.Logger = logger;

			Option<string> option = new Option<string>($"--{nameof(HelloProperties.YourName).ToLower()}", "Full name of person to greet.") {
				IsRequired = true
			};

			this.AddOption(option);

			this.Handler = CommandHandler.Create<HelloProperties>(async (p) => {
				return await this.OnHandleCommand(p);
			});
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<HelloCommand> Logger { get; set; } = new NullLogger<HelloCommand>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="properties"></param>
		/// <returns></returns>
		protected Task<int> OnHandleCommand(HelloProperties properties) {
			this.Logger.LogInformation($"Hello '{properties.YourName}'.");
			return Task.FromResult(0);
		}
	}
}

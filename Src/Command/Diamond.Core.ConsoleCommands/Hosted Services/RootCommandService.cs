using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.ConsoleCommands {
	/// <summary>
	/// 
	/// </summary>
	public class RootCommandService : RootCommand, IHostedService {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="description"></param>
		/// <param name="args"></param>
		public RootCommandService(string description, string[] args)
			: base(description) {
			this.Args = args;
		}

		/// <summary>
		/// 
		/// </summary>
		protected string[] Args { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task StartAsync(CancellationToken cancellationToken) {
			int result = await this.InvokeAsync(this.Args);
			Environment.ExitCode = result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StopAsync(CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}
	}
}

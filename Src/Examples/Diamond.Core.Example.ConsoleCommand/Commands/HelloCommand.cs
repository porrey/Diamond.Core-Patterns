//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
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
	public class HelloCommand : Command
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public HelloCommand(ILogger<HelloCommand> logger)
			: base("hello", "Responds with a hello greeting.")
		{
			this.Logger = logger;

			Option<string> option = new Option<string>($"--{nameof(HelloProperties.YourName).ToLower()}", "Full name of person to greet.")
			{
				IsRequired = true
			};

			this.AddOption(option);

			this.Handler = CommandHandler.Create<HelloProperties>(async (p) =>
			{
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
		protected Task<int> OnHandleCommand(HelloProperties properties)
		{
			this.Logger.LogInformation($"Hello '{properties.YourName}'.");
			return Task.FromResult(0);
		}
	}
}

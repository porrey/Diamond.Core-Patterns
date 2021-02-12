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

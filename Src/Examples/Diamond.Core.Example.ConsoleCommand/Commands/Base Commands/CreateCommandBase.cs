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

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class CreateCommandBase : Command
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public CreateCommandBase(ILogger<CreateCommandBase> logger)
			: base("create", "Creates a new invoice item.")
		{
			this.Logger = logger;

			this.AddOption(new Option<string>($"--{nameof(Invoice.Number).ToLower()}", "Invoice Number.")
			{
				IsRequired = true
			});

			this.AddOption(new Option<string>($"--{nameof(Invoice.Description).ToLower()}", "Invoice Description.")
			{
				IsRequired = true
			});

			this.AddOption(new Option<float>($"--{nameof(Invoice.Total).ToLower()}", "Invoice Total.")
			{
				IsRequired = true
			});

			this.AddOption(new Option<bool>($"--{nameof(Invoice.Paid).ToLower()}", "Indicates if the invoice has been paid or not.")
			{
				IsRequired = false
			});

			this.Handler = CommandHandler.Create<Invoice>(async (p) =>
			{
				return await this.OnHandleCommand(p);
			});
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<CreateCommandBase> Logger { get; set; } = new NullLogger<CreateCommandBase>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual Task<int> OnHandleCommand(Invoice item)
		{
			return Task.FromResult(0);
		}
	}
}

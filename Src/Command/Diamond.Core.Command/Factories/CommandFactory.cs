// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Command
{
	public class CommandFactory : ICommandFactory
	{
		public CommandFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		public ILogger<CommandFactory> Logger { get; set; }
		protected IServiceProvider ServiceProvider { get; set; }

		public Task<ICommand> GetAsync(string commandKey)
		{
			ICommand returnValue = null;

			this.Logger.LogTrace($"Retrieving instance of ICommand with command name '{commandKey}'.");
			IEnumerable<ICommand> commands = this.ServiceProvider.GetRequiredService<IEnumerable<ICommand>>();
			returnValue = commands.Where(t => t.Key == commandKey).SingleOrDefault();

			return Task.FromResult(returnValue);
		}
	}
}

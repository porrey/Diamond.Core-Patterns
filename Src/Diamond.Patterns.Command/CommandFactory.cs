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
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Command
{
	public class CommandFactory : ICommandFactory
	{
		public CommandFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		public CommandFactory(IObjectFactory objectFactory, ILoggerSubscriber loggerSubscriber)
		{
			this.ObjectFactory = objectFactory;
			this.LoggerSubscriber = loggerSubscriber;
		}

		public ILoggerSubscriber LoggerSubscriber { get; set; }
		protected IObjectFactory ObjectFactory { get; set; }

		public Task<ICommand> GetAsync(string commandName)
		{
			ICommand returnValue = null;

			this.LoggerSubscriber.Verbose($"Retrieving instance of ICommand with command name '{commandName}'.");
			returnValue = this.ObjectFactory.GetInstance<ICommand>(commandName);
			this.LoggerSubscriber.AddToInstance(returnValue);

			return Task.FromResult(returnValue);
		}
	}
}

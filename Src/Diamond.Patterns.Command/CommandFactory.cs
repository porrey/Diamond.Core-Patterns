using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Lsc.Logistics.Patterns.Commands
{
	public class CommandFactory : ICommandFactory
	{
		public CommandFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		protected IObjectFactory ObjectFactory { get; set; }

		public Task<ICommand> GetAsync(string commandName)
		{
			ICommand returnValue = null;

			try
			{
				returnValue = this.ObjectFactory.GetInstance<ICommand>(commandName);
			}
			catch
			{
				returnValue = null;
			}

			return Task.FromResult(returnValue);
		}
	}
}

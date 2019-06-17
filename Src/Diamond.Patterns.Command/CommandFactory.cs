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

		protected IObjectFactory ObjectFactory { get; set; }

		public Task<ICommand> GetAsync(string commandName)
		{
			return Task.FromResult(this.ObjectFactory.GetInstance<ICommand>(commandName));
		}
	}
}

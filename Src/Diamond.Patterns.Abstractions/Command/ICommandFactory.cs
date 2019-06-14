using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface ICommandFactory
	{
		Task<ICommand> GetAsync(string parameterSwitch);
	}
}

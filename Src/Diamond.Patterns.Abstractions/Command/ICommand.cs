using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface ICommand
	{
		Task<int> ExecuteAsync();
	}
}

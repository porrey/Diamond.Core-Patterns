using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IRequiresInitialization
	{
		bool CanInitialize { get; }
		bool IsInitialized { get; set; }
		Task<bool> InitializeAsync();
	}
}

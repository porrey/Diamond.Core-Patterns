using Microsoft.EntityFrameworkCore;

namespace Diamond.Patterns.Repository.EntityFrameworkCore
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="optionsBuilder"></param>
	public delegate void OnConfiguringDelegate(DbContextOptionsBuilder optionsBuilder);

	/// <summary>
	/// 
	/// </summary>
	public interface ISupportsConfiguration
	{
		OnConfiguringDelegate ConfiguringCallback { get; set; }
	}
}

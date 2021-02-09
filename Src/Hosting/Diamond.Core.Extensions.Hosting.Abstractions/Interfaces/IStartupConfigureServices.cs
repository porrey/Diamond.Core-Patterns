using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.Hosting
{
	/// <summary>
	/// 
	/// </summary>
	public interface IStartupConfigureServices : IStartup
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		void ConfigureServices(IServiceCollection services);
	}
}

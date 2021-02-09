using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Extensions.Hosting {
	/// <summary>
	/// 
	/// </summary>
	public interface IStartupConfigureContainer : IStartup {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		void ConfigureContainer<TContainer>(TContainer services);
	}
}

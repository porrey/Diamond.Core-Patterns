using Microsoft.Extensions.Configuration;

namespace Diamond.Core.Extensions.Hosting {
	/// <summary>
	/// 
	/// </summary>
	public interface IStartupAppConfiguration : IStartup {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		void ConfigureAppConfiguration(IConfigurationBuilder builder);
	}
}

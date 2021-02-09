using Microsoft.Extensions.Logging;

namespace Diamond.Core.Extensions.Hosting {
	/// <summary>
	/// 
	/// </summary>
	public interface IStartupConfigureLogging : IStartup {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		void ConfigureLogging(ILoggingBuilder builder);
	}
}

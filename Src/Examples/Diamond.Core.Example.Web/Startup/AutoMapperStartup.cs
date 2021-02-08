using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public static class AutoMapperStartup
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddMyAutoMapper(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(Startup));
			return services;
		}
	}
}

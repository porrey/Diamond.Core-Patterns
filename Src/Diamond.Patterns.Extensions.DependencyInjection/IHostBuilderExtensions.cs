using Microsoft.Extensions.Hosting;

namespace Diamond.Patterns.Extensions.DependencyInjection
{
	public static class IHostBuilderExtensions
	{
		public static IHostBuilder UseDiamondDependencyInjection(this IHostBuilder hostBuilder)
		{
			hostBuilder.UseServiceProviderFactory(new DiamondServiceProviderFactory());
			return hostBuilder;
		}
	}
}

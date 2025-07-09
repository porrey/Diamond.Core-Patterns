using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Rules
{
	/// <summary>
	/// Extends the IServiceCollection object.
	/// </summary>
	public static class ServiceCollectionDecorator
	{
		/// <summary>
		/// Adds the necesarys registrations to the service collection.
		/// </summary>
		/// <param name="services"></param>
		public static IServiceCollection UseRulesFactory(this IServiceCollection services)
		{
			services.AddTransient<IRulesFactory, RulesFactory>();
			services.AddSingleton<IServiceCollection>(services);
			return services;
		}
	}
}

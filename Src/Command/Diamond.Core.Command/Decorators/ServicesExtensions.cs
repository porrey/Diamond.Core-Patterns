using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Command
{
	/// <summary>
	/// 
	/// </summary>
	public static class ServicesExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection UseDiamondCommand(this IServiceCollection services)
		{
			// ***
			// *** Add the WorkFlowManagerFactory.
			// ***
			services.AddSingleton<ICommandFactory>(sp =>
			{
				CommandFactory item = new CommandFactory(sp)
				{
					Logger = sp.GetRequiredService<ILogger<CommandFactory>>()
				};

				return item;
			});

			return services;
		}
	}
}

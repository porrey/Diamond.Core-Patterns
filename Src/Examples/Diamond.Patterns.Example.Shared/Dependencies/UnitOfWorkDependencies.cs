using Diamond.Patterns.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Patterns.Example
{
	public static class UnitOfWorkDependencies
	{
		public static IServiceCollection AddUnitOfWorkExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the default dependencies.
			// ***
			services.UseDiamondUnitOfWork();

			// ***
			// *** Add the rules to validate the shipment model. 
			// ***
			services.AddTransient<IUnitOfWork, CreateAppointmentUnitOfWork>();

			return services;
		}
	}
}

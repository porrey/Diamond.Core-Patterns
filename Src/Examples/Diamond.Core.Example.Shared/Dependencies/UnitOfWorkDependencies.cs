using Diamond.Core.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class UnitOfWorkDependencies
	{
		public static IServiceCollection AddUnitOfWorkExampleDependencies(this IServiceCollection services)
		{
			// ***
			// *** Add the rules to validate the shipment model. 
			// ***
			services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>()
					.AddScoped<IUnitOfWork, CreateAppointmentUnitOfWork>();

			return services;
		}
	}
}

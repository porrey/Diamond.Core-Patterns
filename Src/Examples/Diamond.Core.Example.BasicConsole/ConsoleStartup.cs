//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using Diamond.Core.Decorator;
using Diamond.Core.Extensions.Hosting;
using Diamond.Core.Repository;
using Diamond.Core.Rules;
using Diamond.Core.Specification;
using Diamond.Core.UnitOfWork;
using Diamond.Core.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	///
	/// </summary>
	public class ConsoleStartup : IStartupConfigureServices, IStartupAppConfiguration
	{
		/// <summary>
		/// Called to configure additional settings.
		/// </summary>
		/// <param name="builder"></param>
		public void ConfigureAppConfiguration(IConfigurationBuilder builder)
		{
			//
			// Build the configuration so Serilog can read from it.
			//
			IConfigurationRoot configuration = builder.Build();

			//
			// Create a logger from the configuration.
			//
			Log.Logger = new LoggerConfiguration()
					  .ReadFrom.Configuration(configuration)
					  .CreateLogger();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public void ConfigureServices(IServiceCollection services)
		{
			//
			// Add the database context using a transient lifetime.
			//
			services.AddDbContext<SampleContext>((sp, options) =>
			{
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
				options.UseSqlite(configuration["ConnectionStrings:Sample"]);
			}, ServiceLifetime.Transient);

			//
			// Add database items.
			//
			services.UseRepositoryFactory();
			services.AddScoped<IEntityFactory<IEmployeeEntity>, EmployeeEntityFactory>();
			services.AddTransient<IRepository<IEmployeeEntity>, EmployeeRepository>();

			services.UseSpecificationFactory();
			services.AddKeyedTransient<ISpecification, GetActiveEmployeeIdListSpecification>(WellKnown.Specifcation.GetActiveEmployeeIdList);
			services.AddKeyedTransient<ISpecification, GetEmployeeDetailsSpecification>(WellKnown.Specifcation.GetEmployeeDetails);

			services.UseUnitOfWorkFactory();
			services.AddKeyedTransient<IUnitOfWork, CreateEmployeeUnitOfWork>(WellKnown.UnitOfWork.CreateEmployee);
			services.AddKeyedTransient<IUnitOfWork, PromoteEmployeeUnitOfWork>(WellKnown.UnitOfWork.PromoteEmployee);

			services.UseRulesFactory();
			services.AddKeyedTransient<IRule, MnimumEmploymentRule>(WellKnown.Rules.EmployeePromotion);
			services.AddKeyedTransient<IRule, GoodStandingRule>(WellKnown.Rules.EmployeePromotion);
			services.AddKeyedTransient<IRule, PreviousPromotionRule>(WellKnown.Rules.EmployeePromotion);

			services.UseDecoratorFactory();
			services.AddKeyedTransient<IDecorator, EmployeePromotionDecorator>(WellKnown.Decorator.EmployeePromotion);

			//
			// Add the sample work flow manager and work flow steps.
			//
			services.UseWorkflowFactory();

			services.AddKeyedTransient<IWorkflowManager, SampleWorkflowManager>(WellKnown.Workflow.SampleWorkflow)
					.AddKeyedTransient<IWorkflowItem, SampleWorkStep1>(WellKnown.Workflow.SampleWorkflow)
					.AddKeyedTransient<IWorkflowItem, SampleWorkStep2>(WellKnown.Workflow.SampleWorkflow)
					.AddKeyedTransient<IWorkflowItem, SampleWorkStep3>(WellKnown.Workflow.SampleWorkflow)
					.AddKeyedTransient<IWorkflowItem, SampleWorkStep4>(WellKnown.Workflow.SampleWorkflow)
					.AddKeyedTransient<IWorkflowItem, SampleWorkStep5>(WellKnown.Workflow.SampleWorkflow);

			//
			// Add the hosted service.
			//
			services.AddHostedService<BasicExampleHostedService>();
		}
	}
}

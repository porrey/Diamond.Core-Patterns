//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
			services.AddScoped<IRepositoryFactory, RepositoryFactory>();
			services.AddScoped<IEntityFactory<IEmployeeEntity>, EmployeeEntityFactory>();
			services.AddTransient<IRepository<IEmployeeEntity>, EmployeeRepository>();

			services.AddScoped<ISpecificationFactory, SpecificationFactory>();
			services.AddTransient<ISpecification, GetActiveEmployeeIdListSpecification>();
			services.AddTransient<ISpecification, GetEmployeeDetailsSpecification>();

			services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
			services.AddTransient<IUnitOfWork, CreateEmployeeUnitOfWork>();
			services.AddTransient<IUnitOfWork, PromoteEmployeeUnitOfWork>();

			services.AddScoped<IRulesFactory, RulesFactory>();
			services.AddTransient<IRule, MnimumEmploymentRule>();
			services.AddTransient<IRule, GoodStandingRule>();
			services.AddTransient<IRule, PreviousPromotionRule>();

			services.AddScoped<IDecoratorFactory, DecoratorFactory>();
			services.AddTransient<IDecorator, EmployeePromotionDecorator>();

			//
			// Add the sample work flow manager and work flow steps.
			//
			services.AddScoped<IWorkflowManagerFactory, WorkflowManagerFactory>()
					.AddScoped<IWorkflowItemFactory, WorkflowItemFactory>()
					.AddTransient<IWorkflowManager, SampleWorkflowManager>()
					.AddTransient<IWorkflowItem, SampleWorkStep1>()
					.AddTransient<IWorkflowItem, SampleWorkStep2>()
					.AddTransient<IWorkflowItem, SampleWorkStep3>()
					.AddTransient<IWorkflowItem, SampleWorkStep4>()
					.AddTransient<IWorkflowItem, SampleWorkStep5>();

			//
			// Add the hosted service.
			//
			services.AddHostedService<BasicExampleHostedService>();
		}
	}
}

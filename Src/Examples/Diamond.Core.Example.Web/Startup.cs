using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			// ***
			// *** Add the Diamond Patterns dependencies needed for the examples.
			// ***
			services.AddWorkFlowExampleDependencies();
			services.AddSpecificationExampleDependencies();
			services.AddRulesExampleDependencies();
			services.AddDecoratorExampleDependencies();
			services.AddUnitOfWorkExampleDependencies();

			// ***
			// *** Add the example application services.
			// ***
			services.AddHostedService<WorkFlowExampleHostedService>();
			services.AddHostedService<SpecificationExampleHostedService>();
			services.AddHostedService<RulesExampleHostedService>();
			services.AddHostedService<DecoratorExampleHostedService>();
			services.AddHostedService<UnitOfWorkExampleHostedService>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Hello World!");
				});
			});
		}
	}
}

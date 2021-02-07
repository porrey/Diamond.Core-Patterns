using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// ***
			// *** Add the Diamond Core services.
			// ***
			services.AddMyDiamondCore();

			// ***
			// *** Add MVC services.
			// ***
			services.AddMyMvc();

			// ***
			// *** Add the swagger services.
			// ***
			services.AddMySwagger(this.Configuration);

			// ***
			// *** Add versioning services.
			// ***
			services.AddMyVersioning();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=ErpInvoice}/{action=GetInvoice}");
			});

			app.UseMySwagger(this.Configuration);
		}
	}
}

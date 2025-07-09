using Diamond.Core.AspNetCore.Hosting;
using Diamond.Core.Extensions.DependencyInjection;
using Diamond.Core.Extensions.DependencyInjection.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Diamond.Core.Example
{
	/// <summary>
	/// The entry point of the application.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Startup"/> class.
		/// </summary>
		private static Startup _startUp = new();

		/// <summary>
		/// Configures and runs the web application.
		/// </summary>
		/// <remarks>This method sets up the web application by configuring logging with Serilog, loading services
		/// from the "Services" folder, and setting up the web host defaults. It uses the <see cref="Startup"/> class for
		/// further configuration and initializes the application with configured services and database settings before
		/// running it.</remarks>
		/// <param name="args">The command-line arguments used to configure the web application.</param>
		static void Main(string[] args) =>
			WebApplication.CreateBuilder(args)
				.WithHostBuilder((hostBuilder, w) =>
				{
					hostBuilder
						.UseSerilog()
						.ConfigureServicesFolder("Services")
						.ConfigureServices((h, s) =>
						{
							_startUp.Configuration = h.Configuration;
							_startUp.ConfigureServices(s);
						})
						.UseConfiguredServices()
						.UseConfiguredDatabaseServices();
				})
				.Build()
				.ConfigureApp(app =>
				{
					_startUp.Configure(app, app.Environment);
				})
				.Run();

		//
		// The code above is the equivalent of the code above. The model below should be ued
		// when more startup controll is needed. The model above should be used when the startup
		// is simple. Long story short, when using a Starup class, use the code below. When you
		// are not using the Startup class, use the code above omtting the calls to
		// ConfigureServices() and Configure() methods.
		//
		/*
			static void Main(string[] args) =>
				Host.CreateDefaultBuilder(args)
					.UseSerilog()
					.ConfigureServicesFolder("Services")
					.ConfigureWebHostDefaults(webBuilder =>
					{
						webBuilder.UseStartup<Startup>();
					})
					.UseConfiguredServices()
					.UseConfiguredDatabaseServices()
					.Build()
					.Run();
		*/
	}
}
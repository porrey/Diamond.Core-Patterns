using System.Threading.Tasks;
using Diamond.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unity;
using Unity.Microsoft.DependencyInjection;
using UnityBuilder = Unity.Microsoft.DependencyInjection.ServiceProviderExtensions;

namespace Diamond.Core.Example
{
	class Program
	{
		static Task Main(string[] args) => (Host.CreateDefaultBuilder(args)
				.UseUnityServiceProvider()
				.ConfigureContainer<IUnityContainer>(container => Program.ConfigureMyContainer(container))
				.ConfigureServices(services => Program.ConfigureMyServices(services)))
				.RunConsoleAsync();

		private static void ConfigureMyContainer(IUnityContainer container)
		{
		}

		private static void ConfigureMyServices(IServiceCollection services)
		{
			// ***
			// *** Add the default dependencies.
			// ***
			services.UseDiamondRepositoryPattern();

			// ***
			// *** Add the entity factory and repository to the container.
			// ***
			services.AddSingleton<IEntityFactory<IInvoice>, InvoiceEntityFactory>();
			services.AddTransient<IRepository<IInvoice>, InvoiceRepository>();

			// ***
			// *** Get the configuration.
			// ***
			IConfiguration configuration = UnityBuilder.BuildServiceProvider(services).GetRequiredService<IConfiguration>();

			services.AddDbContext<ErpContext>(options =>
			{
				options.UseInMemoryDatabase(configuration["ErpDatabase:InMemory"]);
				//options.UseNpgsql(configuration["ErpDatabase:PostgreSQL"]);
				//options.UseSqlite(configuration["ErpDatabase:SQLite"]);
				//options.UseSqlServer(configuration["ErpDatabase:SqlServer"]);
			});

			services.AddHostedService<RepositoryExampleHostedService>();
		}
	}
}

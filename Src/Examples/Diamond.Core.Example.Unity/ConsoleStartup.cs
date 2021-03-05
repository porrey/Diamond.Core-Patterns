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
using Diamond.Core.Extensions.Hosting;
using Diamond.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class ConsoleStartup : IStartupConfigureServices, IStartupConfigureContainer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="IUnityContainer"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public void ConfigureContainer<IUnityContainer>(IUnityContainer container)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public void ConfigureServices(IServiceCollection services)
		{
			//
			// Add the entity factory and repository to the container.
			//
			services.AddSingleton<IRepositoryFactory, RepositoryFactory>();
			services.AddSingleton<IEntityFactory<IInvoice>, InvoiceEntityFactory>();
			services.AddTransient<IRepository<IInvoice>, InvoiceRepository>();

			services.AddDbContext<ErpContext>((sp, options) =>
			{
				IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
				options.UseInMemoryDatabase(configuration["ConnectionStrings:ERP"]);
			});

			services.AddHostedService<RepositoryExampleHostedService>();
		}
	}
}

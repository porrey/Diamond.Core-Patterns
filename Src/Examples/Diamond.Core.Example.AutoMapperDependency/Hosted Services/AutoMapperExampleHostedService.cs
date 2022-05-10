//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
using AutoMapper;
using Diamond.Core.Example.AutoMapperDependency.Profiles;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.AutoMapperDependency
{
	public class AutoMapperExampleHostedService : HostedServiceTemplate
	{
		public AutoMapperExampleHostedService(ILogger<AutoMapperExampleHostedService> logger, IHostApplicationLifetime appLifetime, IServiceScopeFactory serviceScopeFactory, IMapper mapper)
			: base(appLifetime, logger, serviceScopeFactory)
		{
			this.Mapper = mapper;
		}

		protected int ExitCode { get; set; }
		protected IMapper Mapper { get; set; }

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			this.Logger.LogInformation("Starting {type} service.", nameof(AutoMapperExampleHostedService));

			try
			{
				using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
				{
					SampleModelA modelA = new()
					{
						Name = "John Doe",
						Description = "Employee"
					};

					SampleModelB modelB = this.Mapper.Map<SampleModelB>(modelA);
				}
			}
			catch (Exception ex)
			{
				this.Logger.LogError(ex, "Exception in hosted service.");
				this.ExitCode = 2;
			}
			finally
			{
				this.HostApplicationLifetime.StopApplication();
			}

			return Task.CompletedTask;
		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{
			this.Logger.LogDebug("Exiting service {name} with return code: {code}", nameof(AutoMapperExampleHostedService), this.ExitCode);

			//
			// Exit code.
			//
			Environment.ExitCode = this.ExitCode;
			return Task.CompletedTask;
		}
	}
}

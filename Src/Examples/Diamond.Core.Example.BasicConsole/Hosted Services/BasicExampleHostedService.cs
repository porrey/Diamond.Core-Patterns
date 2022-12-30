//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using System;
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.BasicConsole
{
	public class BasicExampleHostedService : IHostedService
	{
		public BasicExampleHostedService(ILogger<BasicExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
		{
			this.Logger = logger;
			this.HostApplicationLifetime = appLifetime;
			this.Configuration = configuration;
			this.ServiceScopeFactory = serviceScopeFactory;
		}

		protected int ExitCode { get; set; }
		protected ILogger<BasicExampleHostedService> Logger { get; set; }
		protected IHostApplicationLifetime HostApplicationLifetime { get; set; }
		protected IConfiguration Configuration { get; set; }
		protected IServiceScopeFactory ServiceScopeFactory { get; set; }

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			this.Logger.LogInformation("Starting {type} service.", nameof(BasicExampleHostedService));

			try
			{
				using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
				{
					IWorkflowManagerFactory factory = scope.ServiceProvider.GetService<IWorkflowManagerFactory>();

					this.Logger.LogInformation("Retrieving work flow manager '{name}'.", WellKnown.Workflow.SampleWorkflow);
					IWorkflowManager workflowManager = await factory.GetAsync(WellKnown.Workflow.SampleWorkflow);

					this.Logger.LogInformation("Executing work flow manager '{name}'.", WellKnown.Workflow.SampleWorkflow);
					if (await workflowManager.ExecuteWorkflowAsync(new WorkflowContext()))
					{
						this.Logger.LogInformation("Work flow execution was successful.");
						this.ExitCode = 0;
					}
					else
					{
						this.Logger.LogError("Work flow execution failed.");
						this.ExitCode = 1;
					}
				}
			}
			catch (Exception ex)
			{
				this.Logger.LogError(ex, "Exception while executing work flow '{name}'.", WellKnown.Workflow.SampleWorkflow);
				this.ExitCode = 2;
			}
			finally
			{
				this.HostApplicationLifetime.StopApplication();
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			this.Logger.LogDebug("Exiting service {name} with return code: {code}", nameof(BasicExampleHostedService), this.ExitCode);

			//
			// Exit code.
			//
			Environment.ExitCode = this.ExitCode;
			return Task.CompletedTask;
		}
	}
}

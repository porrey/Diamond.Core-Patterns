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
using System;
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.BasicConsole
{
	public class BasicExampleHostedService : IHostedService
	{
		public BasicExampleHostedService(ILogger<BasicExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, IWorkflowManagerFactory workFlowManagerFactory)
		{
			Logger = logger;
			HostApplicationLifetime = appLifetime;
			Configuration = configuration;
			WorkFlowManagerFactory = workFlowManagerFactory;
		}

		protected int ExitCode { get; set; }
		protected ILogger<BasicExampleHostedService> Logger { get; set; }
		protected IHostApplicationLifetime HostApplicationLifetime { get; set; }
		protected IConfiguration Configuration { get; set; }
		protected IWorkflowManagerFactory WorkFlowManagerFactory { get; set; }

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			Logger.LogInformation($"Starting {nameof(BasicExampleHostedService)} service.");

			try
			{
				Logger.LogInformation($"Retrieving work flow manager '{WellKnown.Workflow.SampleWorkflow}'.");
				IWorkflowManager workflowManager = await WorkFlowManagerFactory.GetAsync(WellKnown.Workflow.SampleWorkflow);

				Logger.LogInformation($"Executing work flow manager '{WellKnown.Workflow.SampleWorkflow}'.");
				if (await workflowManager.ExecuteWorkflowAsync(new WorkflowContext()))
				{
					Logger.LogInformation("Work flow execution was successful.");
					ExitCode = 0;
				}
				else
				{
					Logger.LogError("Work flow execution failed.");
					ExitCode = 1;
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, $"Exception while executing work flow '{WellKnown.Workflow.SampleWorkflow}'.");
				ExitCode = 2;
			}
			finally
			{
				this.HostApplicationLifetime.StopApplication();
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			Logger.LogDebug($"Exiting service {nameof(BasicExampleHostedService)} with return code: {ExitCode}");

			//
			// Exit code.
			//
			Environment.ExitCode = ExitCode;
			return Task.CompletedTask;
		}
	}
}

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

namespace Diamond.Core.Example
{
	public class WorkflowExampleHostedService : IHostedService
	{
		private readonly ILogger<WorkflowExampleHostedService> _logger = null;
		private readonly IHostApplicationLifetime _appLifetime = null;
		private readonly IConfiguration _configuration = null;
		private readonly IWorkflowManagerFactory _workFlowManagerFactory = null;

		private int _exitCode = 0;

		public WorkflowExampleHostedService(ILogger<WorkflowExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, IWorkflowManagerFactory workFlowManagerFactory)
		{
			_logger = logger;
			_appLifetime = appLifetime;
			_configuration = configuration;
			_workFlowManagerFactory = workFlowManagerFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Starting {nameof(WorkflowExampleHostedService)} service.");

			try
			{
				_logger.LogInformation($"Retrieving work flow manager '{WellKnown.Workflow.SampleWorkflow}'.");
				IWorkflowManager wk1 = await _workFlowManagerFactory.GetAsync(WellKnown.Workflow.SampleWorkflow);

				_logger.LogInformation($"Executing work flow manager '{WellKnown.Workflow.SampleWorkflow}'.");
				if (await wk1.ExecuteWorkflowAsync(new WorkflowContext()))
				{
					_logger.LogInformation("Work flow execution was successful.");
					_exitCode = 0;
				}
				else
				{
					_logger.LogError("Work flow execution failed.");
					_exitCode = 1;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Exception while executing work flow '{WellKnown.Workflow.SampleWorkflow}'.");
				_exitCode = 2;
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Exiting service {nameof(WorkflowExampleHostedService)} with return code: {_exitCode}");

			//
			// Exit code.
			//
			Environment.ExitCode = _exitCode;
			return Task.CompletedTask;
		}
	}
}

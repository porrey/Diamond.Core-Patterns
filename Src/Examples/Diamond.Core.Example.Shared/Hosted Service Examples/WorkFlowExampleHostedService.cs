using System;
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.WorkFlow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class WorkFlowExampleHostedService : IHostedService
	{
		private readonly ILogger<WorkFlowExampleHostedService> _logger = null;
		private readonly IHostApplicationLifetime _appLifetime = null;
		private readonly IConfiguration _configuration = null;
		private readonly IWorkFlowManagerFactory _workFlowManagerFactory = null;

		private int _exitCode = 0;

		public WorkFlowExampleHostedService(ILogger<WorkFlowExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, IWorkFlowManagerFactory workFlowManagerFactory)
		{
			_logger = logger;
			_appLifetime = appLifetime;
			_configuration = configuration;
			_workFlowManagerFactory = workFlowManagerFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting application.");

			try
			{
				_logger.LogInformation($"Retrieving work flow manager '{WellKnown.WorkFlow.SampleWorkFlow}'.");
				IWorkFlowManager wk1 = await _workFlowManagerFactory.GetAsync(WellKnown.WorkFlow.SampleWorkFlow);

				_logger.LogInformation($"Executing work flow manager '{WellKnown.WorkFlow.SampleWorkFlow}'.");
				if (await wk1.ExecuteWorkflowAsync(new GenericContext()))
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
				_logger.LogError(ex, $"Exception while executing work flow '{WellKnown.WorkFlow.SampleWorkFlow}'.");
				_exitCode = 2;
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Exiting with return code: {_exitCode}");

			// ***
			// *** Exit code.
			// ***
			Environment.ExitCode = _exitCode;
			return Task.CompletedTask;
		}
	}
}

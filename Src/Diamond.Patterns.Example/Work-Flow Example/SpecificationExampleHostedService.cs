using System;
using System.Threading;
using System.Threading.Tasks;
using Diamond.Patterns.WorkFlow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Patterns.Example
{
	public class SpecificationExampleHostedService : IHostedService
	{
		private readonly ILogger<WorkFlowExampleHostedService> _logger = null;
		private readonly IHostApplicationLifetime _appLifetime = null;
		private readonly IConfiguration _configuration = null;
		private readonly IWorkFlowManagerFactory _workFlowManagerFactory = null;

		private int _exitCode = 0;

		public SpecificationExampleHostedService(ILogger<WorkFlowExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, IWorkFlowManagerFactory workFlowManagerFactory)
		{
			_logger = logger;
			_appLifetime = appLifetime;
			_configuration = configuration;
			_workFlowManagerFactory = workFlowManagerFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting application.");

			await RunWorkFlowAsync("Group1");
			await RunWorkFlowAsync("Group2");

			_exitCode = 0;
		}

		private async Task<int> RunWorkFlowAsync(string group)
		{
			int returnValue = 0;

			try
			{
				_logger.LogInformation($"Retrieving work flow manager '{group}'.");
				IWorkFlowManager wk1 = await _workFlowManagerFactory.GetAsync(group);

				_logger.LogInformation($"Executing work flow manager '{group}'.");
				if (await wk1.ExecuteWorkflowAsync(new GenericContext()))
				{
					_logger.LogInformation("Work flow exection was successful.");
				}
				else
				{
					_logger.LogError("Work flow exection failed.");
					returnValue = 1;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Exception while executing work flow '{group}'.");
				returnValue = 2;
			}

			return returnValue;
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

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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Extensions.InterfaceInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Workflow
{
	/// <summary>
	///  This workflow manager executes ALL steps in a work flow. If the
	///  current step indicates it should not be executed it is skipped
	///  and the work flow moves on to the next step.
	/// </summary>
	public class ConditionalWorkflowManager : IWorkflowManager, ILoggerPublisher<ConditionalWorkflowManager>
	{
		/// <summary>
		/// 
		/// </summary>
		public ConditionalWorkflowManager()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="steps"></param>
		public ConditionalWorkflowManager(IWorkflowItem[] steps)
		{
			this.Group = null;
			this.Steps = steps;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="steps"></param>
		/// <param name="group"></param>
		public ConditionalWorkflowManager(IWorkflowItem[] steps, string group)
		{
			this.Group = group;
			this.Steps = steps;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="workFlowItemFactory"></param>
		/// <param name="group"></param>
		public ConditionalWorkflowManager(IWorkflowItemFactory workFlowItemFactory, string group)
		{
			this.Group = group;
			this.WorkflowItemFactory = workFlowItemFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		public IWorkflowItemFactory WorkflowItemFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private IWorkflowItem[] _steps = null;

		/// <summary>
		/// 
		/// </summary>
		public IWorkflowItem[] Steps
		{
			get
			{
				return _steps;
			}
			set
			{
				//
				// Ensure that the states are numbered contiguously.
				//
				if (value.OrderBy(t => t.Ordinal).First().Ordinal == 1)
				{
					bool isContiguous = !value.OrderBy(s => s.Ordinal).Select(t => t.Ordinal).Select((i, j) => i - j).Distinct().Skip(1).Any();

					if (isContiguous)
					{
						//
						// Store the steps ordered by the ordinal property value.
						//
						_steps = value.OrderBy(s => s.Ordinal).ToArray();
					}
					else
					{
						throw new ArgumentOutOfRangeException("The state items for group {group} are not numbered consecutively.", this.Group);
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException("The state items for group {group} must be numbered starting with 1.", this.Group);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ILogger<ConditionalWorkflowManager> Logger { get; set; } = new NullLogger<ConditionalWorkflowManager>();

		/// <summary>
		/// 
		/// </summary>
		public string Group { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task<bool> ExecuteWorkflowAsync(IContext context)
		{
			bool returnValue = true;

			//
			// Initialize the work flow
			//
			await this.LoadAsync();

			//
			// Create a stop watch to time the workflow steps.
			//
			Stopwatch stopWatch = new Stopwatch();

			//
			// Initialize this flag to True.
			//
			context.Properties.Set(DiamondWorkflow.WellKnown.Context.LastStepSuccess, true);
			context.Properties.Set(DiamondWorkflow.WellKnown.Context.WorkflowFailed, false);

			//
			// Loop through each workflow step executing them one at a time.
			//
			for (int i = 0; i < this.Steps.Count(); i++)
			{
				if (this.Steps[i].ShouldExecute(context))
				{
					//
					// Publish a progress update.
					//
					this.Logger.LogDebug("Starting workflow step '{name}' [{i}].", this.Steps[i].Name, $"{i + 1} of {this.Steps.Count()}");

					//
					// Start the stop watch.
					//
					stopWatch.Start();

					//
					//
					//
					bool result = false;

					try
					{
						result = await this.ExecuteStepAsync(this.Steps[i], context);
					}
					finally
					{
						//
						// Stop the stop watch.
						//
						stopWatch.Stop();

						//
						// Check the result.
						//
						if (result)
						{
							context.Properties.Set(DiamondWorkflow.WellKnown.Context.LastStepSuccess, true);
							string time = stopWatch.Elapsed.TotalSeconds < 1.0 ? "< 1 second" : $"{stopWatch.Elapsed.TotalSeconds:#,##0.0}";
							this.Logger.LogDebug("The workflow step '{name}' completed successfully [Execution time = {time} second(s)].", this.Steps[i].Name, time);
							returnValue = true;
						}
						else
						{
							context.Properties.Set(DiamondWorkflow.WellKnown.Context.WorkflowFailed, true);
							context.Properties.Set(DiamondWorkflow.WellKnown.Context.LastStepSuccess, false);
							this.Logger.LogDebug("The workflow step '{name}' failed.", this.Steps[i].Name);
							returnValue = false;
						}

						//
						// Reset the stop watch.
						//
						stopWatch.Reset();
					}
				}
				else
				{
					this.Logger.LogDebug("Skipping workflow step '{name}' [{i}].", this.Steps[i].Name, $"{i + 1} of {this.Steps.Count()}");
				}
			}

			//
			// Check if the context contains a failed flag.
			//
			if (context.Properties.ContainsKey(DiamondWorkflow.WellKnown.Context.WorkflowFailed))
			{
				returnValue = !context.Properties.Get<bool>(DiamondWorkflow.WellKnown.Context.WorkflowFailed);
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected async Task LoadAsync()
		{
			if (this.Steps == null || this.Steps.Count() == 0)
			{
				this.Logger.LogDebug("Loading workflow steps.");
				this.Steps = (await this.WorkflowItemFactory.GetItemsAsync(this.Group)).ToArray();
				this.Logger.LogDebug("{count} steps were loaded.", this.Steps.Count());

				if (this.Steps.Count() == 0)
				{
					this.Logger.LogDebug("Throwing exception because no steps were found for the workflow with group name '{group}'.", this.Group);
					throw new MissingStepsException(this.Group);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="step"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		protected async Task<bool> ExecuteStepAsync(IWorkflowItem step, IContext context)
		{
			bool returnValue = false;

			try
			{
				this.Logger.LogDebug("Executing workflow step '{name}'.", step.Name);
				if (await step.ExecuteStepAsync(context))
				{
					this.Logger.LogDebug("The workflow step '{name}' completed successfully.", step.Name);
					returnValue = true;
				}
				else
				{
					this.Logger.LogError("The workflow step '{name}' failed.", step.Name);

					if (context.HasException())
					{
						this.Logger.LogDebug("The workflow step '{name}' had an exception set.", step.Name);
						context.SetException(new WorkflowFailureException(context.GetException(), step.Name, step.Ordinal));
					}
					else
					{
						this.Logger.LogWarning("The workflow step '{name}' did NOT have an exception set.", step.Name);
						context.SetException(new UnknownFailureException(step.Name, step.Ordinal));
					}
				}
			}
			catch (Exception ex)
			{
				this.Logger.LogError(ex, nameof(ExecuteStepAsync));
				context.SetException(ex);
				returnValue = false;
			}

			return returnValue;
		}
	}
}

//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
using Humanizer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Workflow
{
	/// <summary>
	///  This workflow manager executes ALL steps in a work flow. If the
	///  current step indicates it should not be executed it is skipped
	///  and the work flow moves on to the next step.
	/// </summary>
	public class LinearCompleteWorkflowManager : IWorkflowManager
	{
		/// <summary>
		/// Initializes a new instance of the LinearCompleteWorkflowManager class with the specified logger and workflow item
		/// factory.
		/// </summary>
		/// <param name="logger">The logger used to record diagnostic and operational information for the workflow manager.</param>
		/// <param name="workFlowItemFactory">The factory used to create workflow items managed by this instance.</param>
		public LinearCompleteWorkflowManager(ILogger<LinearCompleteWorkflowManager> logger, IWorkflowItemFactory workFlowItemFactory)
			: this(workFlowItemFactory)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Initializes a new instance of the LinearCompleteWorkflowManager class using the specified workflow item factory.
		/// </summary>
		/// <param name="workFlowItemFactory">The factory used to create workflow items for the manager. Cannot be null.</param>
		public LinearCompleteWorkflowManager(IWorkflowItemFactory workFlowItemFactory)
		{
			this.WorkflowItemFactory = workFlowItemFactory;
		}

		/// <summary>
		/// Gets or sets the factory used to create workflow items.
		/// </summary>
		/// <remarks>Assigning a custom factory allows for customization of workflow item creation. This property is
		/// typically used to inject specialized behavior or dependencies into workflow items.</remarks>
		public virtual IWorkflowItemFactory WorkflowItemFactory { get; set; }

		/// <summary>
		/// Backing field for the <see cref="Steps"/> property. This field holds the collection of workflow steps for the current instance.
		/// </summary>
		private IWorkflowItem[] _steps = null;

		/// <summary>
		/// Gets or sets the collection of workflow steps for the current instance.
		/// </summary>
		/// <remarks>The steps must be numbered contiguously, starting with 1. Setting this property with steps that
		/// do not meet these requirements will result in an exception. The steps are stored in order based on their ordinal
		/// values.</remarks>
		public virtual IWorkflowItem[] Steps
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
						throw new ArgumentOutOfRangeException("The state items for Service Key '{serviceKey}' are not numbered consecutively.", this.ServiceKey);
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException("The state items for Service Key '{serviceKey}' must be numbered starting with 1.", this.ServiceKey);
				}
			}
		}

		/// <summary>
		/// Gets or sets the logger used to record diagnostic and operational information for the workflow manager.
		/// </summary>
		/// <remarks>Assigning a custom logger allows integration with different logging frameworks or configuration
		/// of log output. By default, logging is disabled unless a logger is explicitly provided.</remarks>
		public virtual ILogger<LinearCompleteWorkflowManager> Logger { get; set; } = new NullLogger<LinearCompleteWorkflowManager>();
		
		/// <summary>
		/// Gets or sets the unique key used to identify the service instance.
		/// </summary>
		public virtual string ServiceKey { get; set; }

		/// <summary>
		/// Executes the workflow asynchronously, processing each step in sequence and updating the workflow state in the
		/// provided context.
		/// </summary>
		/// <remarks>The method updates context properties to reflect the outcome of each step and the overall
		/// workflow. Logging is performed for progress and step results. The workflow is executed step-by-step, and failure
		/// in any step will mark the workflow as failed.</remarks>
		/// <param name="context">The context object used to track workflow state and properties throughout execution. Cannot be null.</param>
		/// <returns>A value indicating whether the workflow completed successfully. Returns <see langword="true"/> if all steps
		/// succeeded; otherwise, <see langword="false"/>.</returns>
		public virtual async Task<bool> ExecuteWorkflowAsync(IContext context)
		{
			bool returnValue = true;

			//
			// Initialize the work flow
			//
			await this.LoadAsync();

			//
			// Create a stop watch to time the workflow steps.
			//
			Stopwatch stopWatch = new();

			//
			// Initialize this flag to True.
			//
			context.Properties.Set(DiamondWorkflow.WellKnown.Context.LastStepSuccess, true);
			context.Properties.Set(DiamondWorkflow.WellKnown.Context.WorkflowFailed, false);

			//
			// Loop through each workflow step executing them one at a time.
			//
			for (int i = 0; i < this.Steps.Length; i++)
			{
				//
				// Publish a progress update.
				//
				this.Logger.LogDebug("Starting workflow step '{name}' [{i}].", this.Steps[i].Name, $"{i + 1} of {this.Steps.Length}");

				//
				// Start the stop watch.
				//
				stopWatch.Start();

				//
				// Create a flag for the result.
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
						this.Logger.LogDebug("The workflow step '{name}' completed successfully [Execution time = {time}].", this.Steps[i].Name, stopWatch.Elapsed.Humanize(precision: 3));
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
		/// Asynchronously loads workflow steps if they have not already been loaded.
		/// </summary>
		/// <remarks>This method should be called before accessing workflow steps to ensure they are loaded. If steps
		/// are already loaded, the method completes without reloading.</remarks>
		/// <returns>A task that represents the asynchronous load operation.</returns>
		/// <exception cref="MissingStepsException">Thrown if no steps are found for the workflow associated with the current service key.</exception>
		protected virtual async Task LoadAsync()
		{
			if (this.Steps == null || this.Steps.Length == 0)
			{
				this.Logger.LogDebug("Loading workflow steps.");
				this.Steps = [.. (await this.WorkflowItemFactory.GetItemsAsync(this.ServiceKey))];
				this.Logger.LogDebug("{count} steps were loaded.", this.Steps.Length);

				if (this.Steps.Length == 0)
				{
					this.Logger.LogDebug("Throwing exception because no steps were found for the workflow with Service Key '{serviceKey}'.", this.ServiceKey);
					throw new MissingStepsException(this.ServiceKey);
				}
			}
		}

		/// <summary>
		/// Executes the specified workflow step asynchronously within the given context.
		/// </summary>
		/// <remarks>If the step fails, the method sets an appropriate exception in the context to indicate the
		/// failure reason. Logging is performed for both successful and failed executions.</remarks>
		/// <param name="step">The workflow item to execute. Represents a single step in the workflow.</param>
		/// <param name="context">The execution context for the workflow step. Provides state and exception handling for the step.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the step completed
		/// successfully; otherwise, <see langword="false"/>.</returns>
		protected virtual async Task<bool> ExecuteStepAsync(IWorkflowItem step, IContext context)
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

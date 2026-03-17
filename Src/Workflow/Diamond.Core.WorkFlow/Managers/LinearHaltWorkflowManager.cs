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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Humanizer;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Provides workflow management for executing a sequence of workflow steps in a linear fashion, halting execution upon
	/// the first failure. Supports customization of workflow item creation and logging.
	/// </summary>
	/// <remarks>Use this class to manage workflows where steps must be executed in order and the workflow should
	/// stop if any step fails. The manager ensures steps are numbered contiguously and allows for integration with custom
	/// logging and workflow item factories. The final step can be configured to always execute, regardless of previous
	/// failures. Thread safety is not guaranteed; external synchronization may be required for concurrent
	/// access.</remarks>
	public class LinearHaltWorkflowManager : IWorkflowManager
	{
		/// <summary>
		/// Initializes a new instance of the LinearHaltWorkflowManager class with the specified workflow item factory and
		/// logger.
		/// </summary>
		/// <param name="workFlowItemFactory">The factory used to create workflow items for the manager. Cannot be null.</param>
		/// <param name="logger">The logger used to record diagnostic and operational information for the manager. Cannot be null.</param>
		public LinearHaltWorkflowManager(IWorkflowItemFactory workFlowItemFactory, ILogger<LinearHaltWorkflowManager> logger)
			: this(workFlowItemFactory)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Initializes a new instance of the LinearHaltWorkflowManager class using the specified workflow item factory.
		/// </summary>
		/// <param name="workFlowItemFactory">The factory used to create workflow items for the manager. Cannot be null.</param>
		public LinearHaltWorkflowManager(IWorkflowItemFactory workFlowItemFactory)
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
		/// Gets or sets the logger used to record diagnostic and operational information for the workflow manager.
		/// </summary>
		/// <remarks>Assigning a custom logger allows integration with different logging frameworks or configuration
		/// of log output. By default, logging is disabled unless a logger is explicitly provided.</remarks>
		public virtual ILogger<LinearHaltWorkflowManager> Logger { get; set; } = new NullLogger<LinearHaltWorkflowManager>();

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
				if (_steps == null)
				{
					_steps = this.WorkflowItemFactory.GetItemsAsync(this.ServiceKey).Result.ToArray();

					if (_steps.Count() == 0)
					{
						throw new ArgumentOutOfRangeException($"No work flow items with Service Key '{this.ServiceKey}' were found.");
					}
				}

				return _steps;
			}
			set
			{
				//
				// Ensure that the states are numbered contiguously.
				//
				bool isContiguous = !value.OrderBy(s => s.Ordinal).Select(t => t.Ordinal).Select((i, j) => i - j).Distinct().Skip(1).Any();

				if (!isContiguous)
				{
					string itemOrdinals = String.Join(",", value.Select(t => t.Ordinal));
					throw new ArgumentOutOfRangeException($"The {value.Length} [{itemOrdinals}] state items for Service Key {this.ServiceKey} are not numbered consecutively.");
				}
				else
				{
					//
					// Store the steps ordered by the ordinal property value.
					//
					_steps = value.OrderBy(s => s.Ordinal).ToArray();
				}
			}
		}

		/// <summary>
		/// Gets or sets the unique key used to identify the service instance.
		/// </summary>
		public virtual string ServiceKey { get; set; }

		/// <summary>
		/// Executes the workflow asynchronously, processing each step in sequence and updating the workflow state based on
		/// step outcomes.
		/// </summary>
		/// <remarks>If a step fails, subsequent steps are not executed except for any step marked to always execute
		/// as the final step. The workflow context properties are updated to reflect the outcome of the execution.</remarks>
		/// <param name="context">The workflow execution context containing properties and state information used during workflow processing. Cannot
		/// be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all workflow steps
		/// completed successfully; otherwise, <see langword="false"/>.</returns>
		public virtual async Task<bool> ExecuteWorkflowAsync(IContext context)
		{
			bool returnValue = true;

			//
			// Initialize the work flow
			//
			await this.LoadAsync();

			//
			// Initialize this flag to True.
			//
			context.Properties.Set(DiamondWorkflow.WellKnown.Context.LastStepSuccess, true);
			context.Properties.Set(DiamondWorkflow.WellKnown.Context.WorkflowFailed, false);

			try
			{
				//
				// Create a stop watch to time the workflow steps.
				//
				Stopwatch stopWatch = new();

				//
				// Loop through each workflow step executing them one at a time.
				//
				for (int i = 0; i <= this.FinalStepOfWorkflow; i++)
				{
					this.Logger.LogDebug("Starting workflow step '{name}' [{count}].", this.Steps[i].Name, $"{i + 1} of {this.Steps.Count()}");

					//
					// Start the stop watch.
					//
					stopWatch.Start();

					//
					// Stores the result of the step.
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
							this.Logger.LogDebug("The workflow step '{name}' completed successfully [Execution time = {time}].", this.Steps[i].Name, stopWatch.Elapsed.Humanize(precision: 3));
						}
						else
						{
							returnValue = false;
							this.Logger.LogDebug("The workflow step '{name}' failed.", this.Steps[i].Name);
						}

						//
						// Reset the stop watch.
						//
						stopWatch.Reset();
					}

					//
					// Exit the loop if the last step failed.
					//
					if (!result)
					{
						break;
					}
				}
			}
			finally
			{
				//
				// If the last step of a workflow is marked as final,
				// it should always run as the last step even if the
				// one of the other steps fail.
				//
				if (this.HasAlwaysExecuteStep)
				{
					this.Logger.LogDebug("Starting final workflow step '{name}' [{index} of {count}].", this.Steps[this.AlwaysExecuteStepIndex].Name, this.AlwaysExecuteStepIndex + 1, this.Steps.Count());

					if (await this.ExecuteStepAsync(this.Steps[this.AlwaysExecuteStepIndex], context))
					{
						this.Logger.LogDebug("The final workflow step '{name}' completed successfully.", this.Steps[this.AlwaysExecuteStepIndex].Name);
					}
					else
					{
						this.Logger.LogDebug("The final workflow step '{name}' failed.", this.Steps[this.AlwaysExecuteStepIndex].Name);
					}
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Executes the specified workflow step asynchronously within the given context and determines whether the step
		/// completed successfully.
		/// </summary>
		/// <remarks>If the step fails or an exception occurs during execution, the context is updated with the
		/// relevant exception information. Override this method to customize step execution behavior in derived
		/// classes.</remarks>
		/// <param name="step">The workflow item to execute. Represents a single step in the workflow process.</param>
		/// <param name="context">The execution context for the workflow step. Provides state and exception handling for the step execution.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the step executed
		/// successfully; otherwise, <see langword="false"/>.</returns>
		protected virtual async Task<bool> ExecuteStepAsync(IWorkflowItem step, IContext context)
		{
			bool returnValue = false;

			try
			{
				if (await step.ExecuteStepAsync(context))
				{
					returnValue = true;
				}
				else
				{
					if (context.HasException())
					{
						context.SetException(new WorkflowFailureException(context.GetException(), step.Name, step.Ordinal));
					}
					else
					{
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

		/// <summary>
		/// Asynchronously loads workflow steps for the current group if they have not already been loaded.
		/// </summary>
		/// <remarks>Override this method to customize how workflow steps are loaded for a group. Steps are only
		/// loaded if they have not been previously set.</remarks>
		/// <returns>A task that represents the asynchronous load operation.</returns>
		/// <exception cref="MissingStepsException">Thrown if no steps are found for the specified group.</exception>
		protected virtual async Task LoadAsync()
		{
			if (this.Steps == null || this.Steps.Length == 0)
			{
				this.Steps = [.. (await this.WorkflowItemFactory.GetItemsAsync(this.ServiceKey))];

				if (this.Steps.Length == 0)
				{
					throw new MissingStepsException(this.ServiceKey);
				}
			}
		}

		/// <summary>
		/// Gets the index of the step that is always executed, if present, in the sequence of steps.
		/// </summary>
		/// <remarks>Returns the index of the last step if it is marked to always execute; otherwise, returns -1. This
		/// property is useful for identifying special steps that must run regardless of other conditions.</remarks>
		protected virtual int AlwaysExecuteStepIndex
		{
			get
			{
				int returnValue = -1;

				if (this.Steps[this.Steps.Length - 1].AlwaysExecute)
				{
					returnValue = this.Steps.Length - 1;
				}

				return returnValue;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the last step in the sequence is configured to always execute.
		/// </summary>
		/// <remarks>Use this property to determine if the final step will run regardless of previous outcomes. This
		/// can be useful for ensuring cleanup or finalization actions are performed.</remarks>
		protected virtual bool HasAlwaysExecuteStep
		{
			get
			{
				return this.Steps[this.Steps.Length - 1].AlwaysExecute;
			}
		}

		/// <summary>
		/// Gets the index of the final step in the workflow sequence.
		/// </summary>
		/// <remarks>If the workflow contains an always-execute step marked as final, the index returned corresponds
		/// to the step immediately preceding it. This property is intended for scenarios where the workflow's completion
		/// logic depends on identifying the last actionable step.</remarks>
		protected virtual int FinalStepOfWorkflow
		{
			get
			{
				//
				// The last step of the workflow is the last step,
				// unless it is marked final. Then it is the step
				// before it.
				//
				int returnValue = this.Steps.Length - 1;

				if (this.HasAlwaysExecuteStep)
				{
					returnValue -= 1;
				}

				return returnValue;
			}
		}		
	}
}

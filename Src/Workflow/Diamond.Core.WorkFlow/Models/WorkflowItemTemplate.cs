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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Represents an abstract template for a workflow item, providing base functionality for execution, logging, grouping,
	/// and ordering within a workflow. Derived classes implement specific step behavior by overriding execution methods.
	/// </summary>
	/// <remarks>This class defines the structure and lifecycle for workflow steps, including preparation,
	/// conditional execution, post-execution actions, and error handling. It is intended to be inherited by concrete
	/// workflow item implementations. The workflow manager typically orchestrates the execution of these items. Thread
	/// safety depends on the implementation of derived classes and the logger provided.</remarks>
	public abstract class WorkflowItemTemplate : IWorkflowItem
	{
		/// <summary>
		/// Initializes a new instance of the WorkflowItemTemplate class.
		/// </summary>
		/// <remarks>The Name property is set to the class name with the 'Step' suffix removed during initialization.
		/// This can be useful for identifying workflow items by their template type.</remarks>
		public WorkflowItemTemplate()
		{
			this.Name = this.GetType().Name.Replace("Step", "");
		}

		/// <summary>
		/// Initializes a new instance of the WorkflowItemTemplate class using the specified logger for diagnostic output.
		/// </summary>
		/// <param name="logger">The logger instance used to record diagnostic information and operational events for the WorkflowItemTemplate.</param>
		public WorkflowItemTemplate(ILogger<WorkflowItemTemplate> logger)
			: this()
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Initializes a new instance of the WorkflowItemTemplate class with the specified name, group, and ordinal value.
		/// </summary>
		/// <param name="logger">The logger used for logging diagnostic and operational information within the WorkflowItemTemplate instance.
		/// Cannot be null.</param>
		/// <param name="name">The name assigned to the workflow item template. Cannot be null or empty.</param>
		/// <param name="ordinal">The ordinal value that determines the order of the workflow item template within its group. Must be a non-negative
		/// integer.</param>
		public WorkflowItemTemplate(ILogger<WorkflowItemTemplate> logger, string name, int ordinal)
			: this(logger)
		{
			this.Name = name;
			this.Ordinal = ordinal;
		}

		/// <summary>
		/// Creates an instance of <see cref="WorkflowItemTemplate"/> with the given logger, name, group ordinal and
		/// value for the <see cref="AlwaysExecute"/> property.
		/// </summary>
		/// <param name="logger">An instance of the logger used by this step to create log entries during execution.</param>
		/// <param name="name">The name of the step.</param>
		/// <param name="ordinal">The order in the group this step executes.</param>
		/// <param name="alwaysExecute">Sets the <see cref="AlwaysExecute"/></param> property.
		public WorkflowItemTemplate(ILogger<WorkflowItemTemplate> logger, string name, int ordinal, bool alwaysExecute)
			: this(logger, name, ordinal)
		{
			this.AlwaysExecute = alwaysExecute;
		}

		/// <summary>
		/// Gets/sets the name of this workflow item for logging purposes.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// The order this item appears in the execution steps.
		/// </summary>
		public virtual int Ordinal { get; set; }

		/// <summary>
		/// Gets/sets value to indicate that this step should always execute. This does not
		/// override <see cref="OnShouldExecuteAsync"/> and is used
		/// by some workflow managers but not all.
		/// </summary>
		public virtual bool AlwaysExecute { get; set; } = false;

		/// <summary>
		/// Gets/sets a weigh used for determining priority of this step. This value is used
		/// by some workflow managers but not all.
		/// </summary>
		public virtual double Weight { get; set; } = 1;

		/// <summary>
		/// Gets/sets the instance of the logger used by this 
		/// step to create log entries during execution.
		/// </summary>
		public virtual ILogger<WorkflowItemTemplate> Logger { get; set; } = new NullLogger<WorkflowItemTemplate>();

		/// <summary>
		/// Performs all of the steps of this step's execution including calls
		/// to <see cref="OnPrepareForExecutionAsync"/>, <see cref="OnShouldExecuteAsync"/>
		/// and <see cref="OnPostExecutionAsync"/>. This is usually called by the workflow manager.
		/// </summary>
		/// <param name="context">The current workflow context.</param>
		/// <returns></returns>
		public virtual async Task<bool> ExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			try
			{
				this.Logger.LogDebug("Preparing workflow step '{name}'.", this.Name);
				
				if (await this.OnPrepareForExecutionAsync(context))
				{
					if (await this.OnShouldExecuteAsync(context))
					{
						this.Logger.LogDebug("Executing workflow step '{name}'.", this.Name);
						returnValue = await this.OnExecuteStepAsync(context);
					}
					else
					{
						this.Logger.LogDebug("Skipping workflow step '{name}' based on current context.", this.Name);
						returnValue = true;
					}
				}
				else
				{
					this.Logger.LogError("Failed to prepare workflow Step '{name}'.", this.Name);
				}
			}
			finally
			{
				try
				{
					this.Logger.LogDebug("Running post execution for workflow step '{name}'.", this.Name);
					await this.OnPostExecutionAsync(context);
				}
				catch (Exception ex)
				{
					this.Logger.LogError(ex, "Exception occurred while running post execution for step '{name}.", this.Name);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Called prior to <see cref="OnShouldExecuteAsync"/> and <see cref="OnExecuteStepAsync"/>
		/// to prepare the step for execution.
		/// </summary>
		/// <param name="context">The current workflow context.</param>
		/// <returns>Returns true if preparation for the step execution was
		/// successful; false otherwise.</returns>
		protected virtual Task<bool> OnPrepareForExecutionAsync(IContext context)
		{
			return Task.FromResult(true);
		}

		/// <summary>
		/// Allows the step to dynamically defer execution based on the context.
		/// </summary>
		/// <param name="context">The current workflow context.</param>
		/// <returns>Returns true if the step should execute, false otherwise.</returns>
		public virtual Task<bool> OnShouldExecuteAsync(IContext context)
		{
			return Task.FromResult(true);
		}

		/// <summary>
		/// Called after a step has executed to perform any necessary cleanup.
		/// </summary>
		/// <param name="context">The current workflow context.</param>
		/// <returns>Returns true is the post execution actions were successful; false otherwise.</returns>
		protected virtual Task OnPostExecutionAsync(IContext context)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Performs the actual work of the step execution. This should be overwritten
		/// in the concrete class to perform the necessary actions.
		/// </summary>
		/// <param name="context">The current workflow context.</param>
		/// <returns>Returns true if the step executed successfully; false otherwise.</returns>
		protected virtual Task<bool> OnExecuteStepAsync(IContext context)
		{
			return Task.FromResult(true);
		}

		/// <summary>
		/// Marks a step as failed and provides an error message.
		/// </summary>
		/// <param name="context">The current workflow context.</param>
		/// <param name="message">The message that will be logged to the logging system and the workflow context.</param>
		protected virtual Task StepFailedAsync(IContext context, string message)
		{
			this.Logger.LogDebug("Workflow Step '{name}' failed: '{message}", this.Name, message);
			context.SetException(message);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Returns a string representation of this step.
		/// </summary>
		/// <returns>Returns a string representation of this step.</returns>
		public override string ToString()
		{
			return $"[{this.Ordinal}] {this.Name}";
		}
	}
}

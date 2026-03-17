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
using System.IO;
using System.Threading.Tasks;
using Diamond.Core.System.TemporaryFolder;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Represents a workflow step that deletes a temporary folder created during the workflow execution.
	/// </summary>
	/// <remarks>This step is typically used to clean up temporary resources at the end of a workflow. It relies on
	/// an implementation of ITemporaryFolderFactory to manage folder creation and deletion. The step logs the deletion
	/// process and ensures the temporary folder is removed from the workflow context. If the folder cannot be deleted, a
	/// warning is logged. This class can be customized with additional parameters such as name, group, ordinal, and
	/// alwaysExecute to fit specific workflow requirements.</remarks>
	public class DeleteTemporaryFolderStep : WorkflowItemTemplate
	{
		/// <summary>
		/// Initializes a new instance of the DeleteTemporaryFolderStep class using the specified temporary folder factory.
		/// </summary>
		/// <param name="temporaryFolderFactory">The factory used to create and manage temporary folders for this step. Cannot be null.</param>
		public DeleteTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory)
		{
			this.TemporaryFolderFactory = temporaryFolderFactory;
			this.Name = this.GetType().Name;
		}

		/// <summary>
		/// Initializes a new instance of the DeleteTemporaryFolderStep class with the specified temporary folder factory and
		/// logger.
		/// </summary>
		/// <param name="temporaryFolderFactory">The factory used to create and manage temporary folders for workflow operations. Cannot be null.</param>
		/// <param name="logger">The logger used to record diagnostic information and workflow events. Cannot be null.</param>
		public DeleteTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory, ILogger<WorkflowItemTemplate> logger)
			: this(temporaryFolderFactory)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Initializes a new instance of the DeleteTemporaryFolderStep class with the specified folder factory, logger, name,
		/// group, and ordinal position.
		/// </summary>
		/// <param name="temporaryFolderFactory">The factory used to create and manage temporary folders for the workflow step.</param>
		/// <param name="logger">The logger instance used to record diagnostic and operational information for the workflow item template.</param>
		/// <param name="name">The name assigned to this workflow step. Cannot be null or empty.</param>
		/// <param name="ordinal">The ordinal position of this step within the workflow sequence. Must be a non-negative integer.</param>
		public DeleteTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory, ILogger<WorkflowItemTemplate> logger, string name, int ordinal)
			: this(temporaryFolderFactory, logger)
		{
			this.Name = name;
			this.Ordinal = ordinal;
		}

		/// <summary>
		/// Initializes a new instance of the DeleteTemporaryFolderStep class with the specified configuration and execution
		/// behavior.
		/// </summary>
		/// <param name="temporaryFolderFactory">The factory used to create and manage temporary folders for workflow operations.</param>
		/// <param name="logger">The logger instance used to record diagnostic and workflow-related messages.</param>
		/// <param name="name">The name assigned to this workflow step. Used for identification within the workflow.</param>
		/// <param name="ordinal">The ordinal position of this step in the workflow sequence. Determines execution order.</param>
		/// <param name="alwaysExecute">A value indicating whether this step should execute regardless of workflow conditions. Set to <see
		/// langword="true"/> to force execution.</param>
		public DeleteTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory, ILogger<WorkflowItemTemplate> logger, string name, int ordinal, bool alwaysExecute)
			: this(temporaryFolderFactory, logger, name, ordinal)
		{
			this.AlwaysExecute = alwaysExecute;
		}

		/// <summary>
		/// Gets or sets the display name for the operation.
		/// </summary>
		public override string Name { get; set; } = "Delete Temporary Folder";

		/// <summary>
		/// Gets or sets the factory used to create temporary folders for file operations.
		/// </summary>
		/// <remarks>Override this property to customize how temporary folders are created or managed. The factory
		/// should implement strategies for creating, cleaning up, and managing temporary storage as needed by the
		/// application.</remarks>
		protected virtual ITemporaryFolderFactory TemporaryFolderFactory { get; set; }

		/// <summary>
		/// Executes the step to delete the temporary folder associated with the current workflow context.
		/// </summary>
		/// <remarks>If a temporary folder is present in the context, it is disposed and removed. Logging is performed
		/// to indicate whether the folder was successfully deleted. The method always returns <see
		/// langword="true"/>.</remarks>
		/// <param name="context">The workflow context containing properties and state for the current execution step. Must not be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> when the step
		/// completes.</returns>
		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			if (context.Properties.ContainsKey(DiamondWorkflow.WellKnown.Context.TemporaryFolder))
			{
				ITemporaryFolder temporaryFolder = context.Properties.Get<ITemporaryFolder>(DiamondWorkflow.WellKnown.Context.TemporaryFolder);

				//
				// Cache the name since the object is being disposed.
				//
				string tempPath = temporaryFolder.FullPath;
				this.Logger.LogDebug("Deleting temporary folder '{tempPath}'.", tempPath);

				TryDisposable<ITemporaryFolder>.Dispose(temporaryFolder);
				context.Properties.Remove(DiamondWorkflow.WellKnown.Context.TemporaryFolder);

				if (Directory.Exists(tempPath))
				{
					this.Logger.LogWarning("The temporary folder '{path}' could not be deleted.", tempPath);
				}
				else
				{
					this.Logger.LogDebug("The temporary folder '{path}' was deleted successfully.", tempPath);
				}
			}

			//
			// Always return true.
			//
			return Task.FromResult(true);
		}
	}
}

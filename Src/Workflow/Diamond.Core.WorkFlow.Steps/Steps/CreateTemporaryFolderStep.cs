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
using System.IO;
using System.Threading.Tasks;
using Diamond.Core.System.TemporaryFolder;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Represents a workflow step that creates a temporary folder and stores it in the workflow context.
	/// </summary>
	/// <remarks>This step is typically used in automated workflows that require a dedicated temporary storage
	/// location. The created folder is accessible to subsequent workflow steps via the context properties. Thread safety
	/// depends on the implementation of the underlying temporary folder factory.</remarks>
	public class CreateTemporaryFolderStep : WorkflowItemTemplate
	{
		/// <summary>
		/// Initializes a new instance of the CreateTemporaryFolderStep class using the specified temporary folder factory.
		/// </summary>
		/// <param name="temporaryFolderFactory">The factory used to create temporary folders for this step. Cannot be null.</param>
		public CreateTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory)
		{
			this.TemporaryFolderFactory = temporaryFolderFactory;
			this.Name = this.GetType().Name;
		}

		/// <summary>
		/// Initializes a new instance of the CreateTemporaryFolderStep class with the specified temporary folder factory and
		/// logger.
		/// </summary>
		/// <param name="temporaryFolderFactory">The factory used to create temporary folders for workflow operations. Cannot be null.</param>
		/// <param name="logger">The logger used to record diagnostic information and workflow events. Cannot be null.</param>
		public CreateTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory, ILogger<WorkflowItemTemplate> logger)
			: this(temporaryFolderFactory)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Initializes a new instance of the CreateTemporaryFolderStep class with the specified folder factory, logger, name,
		/// group, and ordinal.
		/// </summary>
		/// <param name="temporaryFolderFactory">The factory used to create temporary folders for this workflow step. Cannot be null.</param>
		/// <param name="logger">The logger instance used for logging workflow item template operations. Cannot be null.</param>
		/// <param name="name">The name assigned to this workflow step. Cannot be null or empty.</param>
		/// <param name="ordinal">The ordinal position of this step within the workflow. Must be a non-negative integer.</param>
		public CreateTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory, ILogger<WorkflowItemTemplate> logger, string name, int ordinal)
			: this(temporaryFolderFactory, logger)
		{
			this.Name = name;
			this.Ordinal = ordinal;
		}

		/// <summary>
		/// Initializes a new instance of the CreateTemporaryFolderStep class with the specified configuration and execution
		/// behavior.
		/// </summary>
		/// <param name="temporaryFolderFactory">The factory used to create temporary folders for the workflow step. Cannot be null.</param>
		/// <param name="logger">The logger instance used for logging workflow item template events. Cannot be null.</param>
		/// <param name="name">The name assigned to the workflow step. Used for identification within the workflow.</param>
		/// <param name="ordinal">The ordinal position of the step within the workflow sequence. Must be a non-negative integer.</param>
		/// <param name="alwaysExecute">A value indicating whether the step should always execute, regardless of workflow conditions. Set to <see
		/// langword="true"/> to force execution.</param>
		public CreateTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory, ILogger<WorkflowItemTemplate> logger, string name, int ordinal, bool alwaysExecute)
			: this(temporaryFolderFactory, logger, name, ordinal)
		{
			this.AlwaysExecute = alwaysExecute;
		}

		/// <summary>
		/// Gets or sets the display name for the operation.
		/// </summary>
		public override string Name { get; set; } = "Create Temporary Folder";

		/// <summary>
		/// Gets or sets the factory used to create temporary folders for file operations.
		/// </summary>
		/// <remarks>Override this property to customize how temporary folders are created or managed. The factory
		/// should implement strategies appropriate for the application's environment and requirements.</remarks>
		protected virtual ITemporaryFolderFactory TemporaryFolderFactory { get; set; }

		/// <summary>
		/// Executes the step by creating a temporary folder and updating the context with its reference.
		/// </summary>
		/// <remarks>If the temporary folder cannot be created, the step is marked as failed and the context is not
		/// updated. The method logs the creation of the folder for diagnostic purposes.</remarks>
		/// <param name="context">The workflow context used to store properties and track execution state.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the temporary
		/// folder was successfully created and set in the context; otherwise, <see langword="false"/>.</returns>
		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			ITemporaryFolder temporaryFolder = this.TemporaryFolderFactory.Create($"{{0}}{this.Name}.{{1}}");
			this.Logger.LogDebug("Created temporary folder '{path}'.", temporaryFolder.FullPath);

			if (Directory.Exists(temporaryFolder.FullPath))
			{
				context.Properties.Set(DiamondWorkflow.WellKnown.Context.TemporaryFolder, temporaryFolder);
				returnValue = true;
			}
			else
			{
				this.StepFailedAsync(context, "Failed to create temporary folder.");
			}

			return Task.FromResult(returnValue);
		}
	}
}

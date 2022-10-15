//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
	/// 
	/// </summary>
	public class DeleteTemporaryFolderStep : WorkflowItemTemplate
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="temporaryFolderFactory"></param>
		public DeleteTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory)
		{
			this.TemporaryFolderFactory = temporaryFolderFactory;
			this.Name = this.GetType().Name;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="temporaryFolderFactory"></param>
		/// <param name="logger"></param>
		public DeleteTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory, ILogger<WorkflowItemTemplate> logger)
			: this(temporaryFolderFactory)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="temporaryFolderFactory"></param>
		/// <param name="logger"></param>
		/// <param name="name"></param>
		/// <param name="group"></param>
		/// <param name="ordinal"></param>
		public DeleteTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory, ILogger<WorkflowItemTemplate> logger, string name, string group, int ordinal)
			: this(temporaryFolderFactory, logger)
		{
			this.Name = name;
			this.Group = group;
			this.Ordinal = ordinal;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="temporaryFolderFactory"></param>
		/// <param name="logger"></param>
		/// <param name="name"></param>
		/// <param name="group"></param>
		/// <param name="ordinal"></param>
		/// <param name="alwaysExecute"></param>
		public DeleteTemporaryFolderStep(ITemporaryFolderFactory temporaryFolderFactory, ILogger<WorkflowItemTemplate> logger, string name, string group, int ordinal, bool alwaysExecute)
			: this(temporaryFolderFactory, logger, name, group, ordinal)
		{
			this.AlwaysExecute = alwaysExecute;
		}

		/// <summary>
		/// 
		/// </summary>
		public override string Name { get; set; } = "Delete Temporary Folder";

		/// <summary>
		/// 
		/// </summary>
		protected virtual ITemporaryFolderFactory TemporaryFolderFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context">The current workflow context.</param>
		/// <returns></returns>
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

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
using System.IO;
using System.Threading.Tasks;
using Diamond.Core.System.TemporaryFolder;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// 
	/// </summary>
	public class DeleteTemporaryFolderStep : WorkflowItem
	{
		/// <summary>
		/// 
		/// </summary>
		public override string Name => "Delete Temporary Folder";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
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
				this.Logger.LogDebug($"Deleting temporary folder '{tempPath}'.");

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

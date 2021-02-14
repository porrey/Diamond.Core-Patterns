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
using System.IO;
using System.Threading.Tasks;
using Diamond.Core.System;
using Microsoft.Extensions.Logging;

#pragma warning disable DF0010

namespace Diamond.Core.WorkFlow {
	/// <summary>
	/// 
	/// </summary>
	public class CreateTemporaryFolderStep : WorkFlowItem {
		/// <summary>
		/// 
		/// </summary>
		public override string Name => "Create Temporary Folder";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override Task<bool> OnExecuteStepAsync(IContext context) {
			bool returnValue = false;

			ITemporaryFolder temporaryFolder = TemporaryFolder.Factory.Create("{0}DynaMailCmd.{1}");
			this.Logger.LogDebug("Created temporary folder '{0}'.", temporaryFolder.FullPath);

			if (Directory.Exists(temporaryFolder.FullPath)) {
				context.Properties.Set(DiamondWorkFlow.WellKnown.Context.TemporaryFolder, temporaryFolder);
				returnValue = true;
			}
			else {
				this.StepFailedAsync(context, "Failed to create temporary folder.");
			}

			return Task.FromResult(returnValue);
		}
	}
}

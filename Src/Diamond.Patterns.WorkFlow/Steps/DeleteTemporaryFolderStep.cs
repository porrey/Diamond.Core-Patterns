// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	public class DeleteTemporaryFolderStep<TContextDecorator, TContext> : WorkFlowItem<TContextDecorator, TContext>
		where TContext : IContext
		where TContextDecorator : IContextDecorator<TContext>
	{
		public override string Name => "Delete Temporary Folder";

		protected override Task<bool> OnExecuteStepAsync(TContextDecorator context)
		{
			if (context.Properties.ContainsKey(DiamondWorkFlow.WellKnown.Context.TemporaryFolder))
			{
				ITemporaryFolder temporaryFolder = context.Properties.Get<ITemporaryFolder>(DiamondWorkFlow.WellKnown.Context.TemporaryFolder);

				// ***
				// *** Cache the name since the object is being disposed.
				// ***
				string tempPath = temporaryFolder.FullPath;
				Trace.TraceInformation($"Deleting temporary folder '{tempPath}'.");

				TryDisposable<ITemporaryFolder>.Dispose(temporaryFolder);
				context.Properties.Remove(DiamondWorkFlow.WellKnown.Context.TemporaryFolder);

				if (Directory.Exists(tempPath))
				{
					Trace.TraceWarning("The temporary folder '{0}' could not be deleted.", tempPath);
				}
				else
				{
					Trace.TraceInformation("The temporary folder '{0}' was deleted successfully.", tempPath);
				}
			}

			// ***
			// *** Always return true.
			// ***
			return Task.FromResult(true);
		}
	}
}

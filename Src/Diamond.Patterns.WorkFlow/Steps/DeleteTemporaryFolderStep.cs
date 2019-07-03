using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	public class DeleteTemporaryFolderStep<TContext> : WorkFlowItem<TContext> where TContext : IContext
	{
		public override string Name => "Delete Temporary Folder";

		protected override Task<bool> OnExecuteStepAsync(IContextDecorator<TContext> context)
		{
			if (context.Properties.ContainsKey(WellKnown.Context.TemporaryFolder))
			{
				ITemporaryFolder temporaryFolder = context.Properties.Get<ITemporaryFolder>(WellKnown.Context.TemporaryFolder);

				// ***
				// *** Cache the name since the object is being disposed.
				// ***
				string tempPath = temporaryFolder.FullPath;
				Trace.TraceInformation($"Deleting temporary folder '{tempPath}'.");

				TryDisposable<ITemporaryFolder>.Dispose(temporaryFolder);
				context.Properties.Remove(WellKnown.Context.TemporaryFolder);

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

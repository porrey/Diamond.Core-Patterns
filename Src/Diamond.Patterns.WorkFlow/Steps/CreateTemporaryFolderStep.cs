using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Diamond.Patterns.Core;

namespace Diamond.Patterns.WorkFlow
{
	public class CreateTemporaryFolderStep<TContext> : WorkFlowItem<TContext> where TContext : IContext
	{
		public override string Name => "Create Temporary Folder";

		protected override Task<bool> OnExecuteStepAsync(IContextDecorator<TContext> context)
		{
			bool returnValue = false;

			ITemporaryFolder temporaryFolder = TemporaryFolder.Factory.Create("{0}DynaMailCmd.{1}");
			Trace.TraceWarning("Created temporary folder '{0}'.", temporaryFolder.FullPath);

			if (Directory.Exists(temporaryFolder.FullPath))
			{
				context.Properties.Set(WellKnown.Context.TemporaryFolder, temporaryFolder);
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

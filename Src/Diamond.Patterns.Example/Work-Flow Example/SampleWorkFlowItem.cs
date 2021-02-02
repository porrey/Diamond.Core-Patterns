using System.Threading.Tasks;
using Diamond.Patterns.WorkFlow;
using Microsoft.Extensions.Logging;

namespace Diamond.Patterns.Example
{
	public class SampleWorkFlowItem : WorkFlowItem
	{
		public override string Name => $"Sample Step {this.Ordinal}";

		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			this.Logger.LogDebug($"Running '{nameof(OnExecuteStepAsync)}' on step [{this.Ordinal}] {this.Group}/{this.Name}.");
			return Task.FromResult(true);
		}

		protected override Task<bool> OnPrepareForExecutionAsync(IContext context)
		{
			this.Logger.LogDebug($"Running '{nameof(OnPrepareForExecutionAsync)}' on step [{this.Ordinal}] {this.Group}/{this.Name}.");
			return Task.FromResult(true);
		}
	}
}

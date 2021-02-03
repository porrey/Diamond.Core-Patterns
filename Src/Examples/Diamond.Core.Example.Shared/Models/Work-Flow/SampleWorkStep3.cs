using System.Threading.Tasks;
using Diamond.Core.WorkFlow;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class SampleWorkStep3 : WorkFlowItem
	{
		public override string Name => $"Sample Step {this.Ordinal}";
		public override string Group { get => WellKnown.WorkFlow.SampleWorkFlow; set => base.Group = value; }
		public override int Ordinal { get => 3; set => base.Ordinal = value; }

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

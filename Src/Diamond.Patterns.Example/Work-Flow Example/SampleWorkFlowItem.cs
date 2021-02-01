using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Diamond.Patterns.WorkFlow;
using Microsoft.Extensions.Logging;

namespace Diamond.Patterns.Example
{
	public class SampleWorkFlowItem : WorkFlowItem
	{
		public SampleWorkFlowItem(string group, int ordinal, string name)
		{
			this.Name = name;
			this.Ordinal = ordinal;
			this.Group = group;
		}

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

using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Diamond.Patterns.Context;

namespace Diamond.Patterns.WorkFlow
{
	public abstract class WorkFlowItem<TContextDecorator, TContext> : IWorkFlowItem<TContextDecorator, TContext>
		where TContext : IContext
		where TContextDecorator : IContextDecorator<TContext>
	{
		/// <summary>
		/// Gets/sets the name of this work-flow item for logging purposes.
		/// </summary>
		public virtual string Name { get; set; } = "Name Not Set";

		/// <summary>
		/// Gets/sets the group this item belongs to. Items are grouped together
		/// so that the WorkFlowManager can gather the steps into a workable series.
		/// </summary>
		public virtual string Group { get; set; }

		/// <summary>
		/// The order this item appears in the execution steps.
		/// </summary>
		public virtual int Ordinal { get; set; }

		public virtual bool AlwaysExecute { get; set; } = false;
		public double Weight { get; set; } = 1;

		public virtual bool ShouldExecute(TContextDecorator context)
		{
			return true;
		}

		public virtual async Task<bool> ExecuteStepAsync(TContextDecorator context)
		{
			bool returnValue = false;

			if (await this.OnPrepareForExecutionAsync(context))
			{
				returnValue = await this.OnExecuteStepAsync(context);
			}

			return returnValue;
		}

		protected virtual Task<bool> OnPrepareForExecutionAsync(TContextDecorator context)
		{
			return Task.FromResult(true);
		}

		protected virtual Task<bool> OnExecuteStepAsync(TContextDecorator context)
		{
			return Task.FromResult(true);
		}

		public override string ToString()
		{
			return $"[{this.Ordinal}] {this.Name} | Group: {this.Group}";
		}

		protected Task StepFailedAsync(TContextDecorator context, string message)
		{
			context.SetException(message);
			return Task.FromResult(0);
		}
	}
}

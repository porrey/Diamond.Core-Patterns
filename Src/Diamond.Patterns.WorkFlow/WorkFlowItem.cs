using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	public abstract class WorkFlowItem<TContext> : IWorkFlowItem<TContext> where TContext : IContext
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

		public virtual async Task<bool> ExecuteStepAsync(IContextDecorator<TContext> context)
		{
			bool returnValue = false;

			if (await this.OnPrepareForExecutionAsync(context))
			{
				returnValue = await this.OnExecuteStepAsync(context);
			}

			return returnValue;
		}

		protected virtual Task<bool> OnPrepareForExecutionAsync(IContextDecorator<TContext> context)
		{
			return Task.FromResult(true);
		}

		protected virtual Task<bool> OnExecuteStepAsync(IContextDecorator<TContext> context)
		{
			return Task.FromResult(true);
		}

		public override string ToString()
		{
			return $"[{this.Ordinal}] {this.Name} | Group: {this.Group}";
		}
	}
}

using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	public class IsErrorStep<TContextDecorator, TContext> : WorkFlowItem<TContextDecorator, TContext>
		where TContext : IContext
		where TContextDecorator : IContextDecorator<TContext>
	{
		public override string Name => "Work-Flow Is In Error";
		protected IRepositoryFactory RepositoryFactory { get; set; }

		protected override Task<bool> OnExecuteStepAsync(TContextDecorator context)
		{
			bool returnValue = false;

			if (context.Properties.ContainsKey(DiamondWorkFlow.WellKnown.Context.WorkFlowError))
			{
				// ***
				// *** Return true to allow the work-flow to continue and false to stop it.
				// ***
				if (context.Properties.Get<bool>(DiamondWorkFlow.WellKnown.Context.WorkFlowError))
				{
					if (context.Properties.ContainsKey(DiamondWorkFlow.WellKnown.Context.WorkFlowErrorMessage))
					{
						this.StepFailedAsync(context, context.Properties.Get<string>(DiamondWorkFlow.WellKnown.Context.WorkFlowErrorMessage));
					}
					else
					{
						this.StepFailedAsync(context, "The error that caused this work-flow to fail was not specified.");
					}

					returnValue = false;
				}
				else
				{
					// ***
					// *** No error, allow the work-flow to continue.
					// ***
					returnValue = true;
				}
			}
			else
			{
				// ***
				// *** Allow the work-flow to continue.
				// ***
				returnValue = true;
			}

			return Task.FromResult(returnValue);
		}
	}
}

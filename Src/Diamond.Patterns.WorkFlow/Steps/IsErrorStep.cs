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

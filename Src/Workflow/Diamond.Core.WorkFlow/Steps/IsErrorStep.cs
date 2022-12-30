//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
// 
using System.Threading.Tasks;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// 
	/// </summary>
	public class IsErrorStep : WorkflowItemTemplate
	{
		/// <summary>
		/// 
		/// </summary>
		public override string Name => "Workflow Is In Error";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context">The current workflow context.</param>
		/// <returns></returns>
		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			if (context.Properties.ContainsKey(DiamondWorkflow.WellKnown.Context.WorkflowError))
			{
				//
				// Return true to allow the workflow to continue and false to stop it.
				//
				if (context.Properties.Get<bool>(DiamondWorkflow.WellKnown.Context.WorkflowError))
				{
					if (context.Properties.ContainsKey(DiamondWorkflow.WellKnown.Context.WorkflowErrorMessage))
					{
						this.StepFailedAsync(context, context.Properties.Get<string>(DiamondWorkflow.WellKnown.Context.WorkflowErrorMessage));
					}
					else
					{
						this.StepFailedAsync(context, "The error that caused this workflow to fail was not specified.");
					}

					returnValue = false;
				}
				else
				{
					//
					// No error, allow the workflow to continue.
					//
					returnValue = true;
				}
			}
			else
			{
				//
				// Allow the workflow to continue.
				//
				returnValue = true;
			}

			return Task.FromResult(returnValue);
		}
	}
}

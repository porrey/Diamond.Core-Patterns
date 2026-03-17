//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
	/// Represents a workflow step that checks whether the workflow is in an error state.
	/// </summary>
	/// <remarks>This class is used within a workflow to determine if an error has occurred and to handle error
	/// conditions appropriately. If an error is detected, the workflow step will fail and provide an error message if
	/// available. Otherwise, the workflow continues as normal.</remarks>
	public class IsErrorStep : WorkflowItemTemplate
	{
		/// <summary>
		/// Gets the display name for the error state of the workflow.
		/// </summary>
		public override string Name => "Workflow Is In Error";

		/// <summary>
		/// Executes the workflow step asynchronously and determines whether the workflow should continue based on the error
		/// state in the context.
		/// </summary>
		/// <remarks>If the context contains a workflow error, the step is marked as failed and the workflow will not
		/// continue. If no error is present, the workflow proceeds to the next step.</remarks>
		/// <param name="context">The workflow execution context containing properties that influence step execution, including error information.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the workflow
		/// should continue; otherwise, <see langword="false"/>.</returns>
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

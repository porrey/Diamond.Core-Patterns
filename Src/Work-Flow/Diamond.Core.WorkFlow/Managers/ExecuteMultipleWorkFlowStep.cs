// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Abstractions;
using Diamond.Core.WorkFlow;

namespace Diamond.Core.WorkFlow
{
	//public class ExecuteMultipleWorkFlowStep<T> : WorkFlowItem<T> where T : IContext
	//{
	//	public ExecuteMultipleWorkFlowStep(IObjectFactory objectFactory, string workFlowManagerName, string workflowTitle, bool stopOnError)
	//	{
	//		this.ObjectFactory = objectFactory;
	//		this.WorkFlowManagerName = workFlowManagerName;
	//		this.WorkflowTitle = workflowTitle;
	//		this.StopOnError = stopOnError;
	//	}

	//	public override string Name => "Execute Multiple Work Flow";

	//	IObjectFactory ObjectFactory { get; set; }
	//	protected string WorkFlowManagerName { get; set; }
	//	protected string WorkflowTitle { get; set; }
	//	protected bool StopOnError { get; set; }

	//	protected override async Task<bool> OnExecuteStepAsync(IContextDecorator<T> context)
	//	{
	//		bool returnValue = true;

	//		if (context.Properties.ContainsKey(DiamondWorkFlow.WellKnown.Context.IStateDictionaryArray))
	//		{
	//			// ***
	//			// *** The context should have any array of IStateDictionary
	//			// *** to run multiple work flows. Each array item is an
	//			// *** instance of the work flow and the dictionary gets
	//			// *** added to the context before the work flow starts
	//			// *** and removed from the context after the work flow completes.
	//			// ***
	//			IStateDictionary[] instances = context.Properties.Get<IStateDictionary[]>(DiamondWorkFlow.WellKnown.Context.IStateDictionaryArray);

	//			// ***
	//			// *** Track the number of work flow failures.
	//			// ***
	//			int errorCount = 0;

	//			// ***
	//			// *** Get the work flow
	//			// ***
	//			//context.EventManager.PublishInformationEvent(this.Name, $"Loading work flow '{this.WorkflowTitle}'.");
	//			IWorkFlowManager<T> workFlowManager = this.ObjectFactory.GetInstance<IWorkFlowManager<T>>(this.WorkFlowManagerName);

	//			foreach (IStateDictionary instance in instances)
	//			{
	//				// ***
	//				// *** Add the iteration instance context properties
	//				// *** to the current context.
	//				// ***
	//				await context.Properties.Merge(instance);

	//				// ***
	//				// *** Execute the work flow
	//				// ***
	//				Trace.TraceInformation($"Running the '{this.WorkflowTitle}' work flow {instances.Count()} times."));

	//				bool executionResult = await workFlowManager.ExecuteWorkflowAsync(context);
	//				bool workFlowFailed = context.Properties.Get<bool>(DiamondWorkFlow.WellKnown.Context.WorkFlowFailed);

	//				if (executionResult && !workFlowFailed)
	//				{
	//					Trace.TraceInformation($"{this.Name} completed successfully.");
	//				}
	//				else
	//				{
	//					Trace.WriteLine($"{this.Name} completed with errors.", "Verbose");

	//					if (context is IExceptionContext exceptionContext)
	//					{
	//						Trace.WriteLine($"Exception: '{exceptionContext.Exception.Message}'.", "Verbose");
	//					}
	//					else
	//					{
	//						Trace.WriteLine($"{this.Name} completed with errors.", "Verbose");
	//					}

	//					// ***
	//					// *** Check if we should quit or keep going.
	//					// ***
	//					if (this.StopOnError)
	//					{
	//						break;
	//					}
	//				}

	//				// ***
	//				// *** Remove the instance properties from the current context.
	//				// ***
	//				await context.Properties.Remove(instance);
	//			}

	//			// ***
	//			// *** Check if we have errors
	//			// ***
	//			if (errorCount == 0)
	//			{
	//				returnValue = true;
	//			}
	//			else
	//			{
	//				await this.StepFailedAsync(context, "One or more instances of the work flow failed.");
	//			}
	//		}
	//		else
	//		{
	//			await this.StepFailedAsync(context, new MissingContextPropertyException(DiamondWorkFlow.WellKnown.Context.IStateDictionaryArray));
	//		}

	//		return returnValue;
	//	}
	//}
}

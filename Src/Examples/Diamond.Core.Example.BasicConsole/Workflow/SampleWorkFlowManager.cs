//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// In a linear complete workflow, all steps are executed regardless
	/// if any previous step has failed. A step can indicate if it
	/// should be executed or not by overriding ShouldExecute().
	/// </summary>
	public class SampleWorkflowManager : LinearCompleteWorkflowManager
	{
		public SampleWorkflowManager(ILogger<SampleWorkflowManager> logger, IWorkflowItemFactory workFlowItemFactory)
			: base(workFlowItemFactory, logger)
		{
			this.Group = WellKnown.Workflow.SampleWorkflow;
			logger.LogDebug($"An instance of {nameof(SampleWorkflowManager)} with group name '{this.Group}' was created.");
		}
	}
}

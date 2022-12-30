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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// This step will create random employee data.
	/// </summary>
	public class SampleWorkStep2 : WorkflowItemTemplate
	{
		public SampleWorkStep2(ILogger<SampleWorkStep2> logger)
			: base(logger)
		{
		}

		public override string Name => $"Sample Step {this.Ordinal}";
		public override string Group => WellKnown.Workflow.SampleWorkflow;
		public override int Ordinal => 2;

		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Create a random number generator instance.
			//
			Random rnd = new();

			//
			// Read the names file and create an EmployeeEntity instance for each.
			//
			IEnumerable<EmployeeEntity> items = from tbl in File.ReadLines("./Data/names.txt")
												let x = tbl.Trim().Split(" ")
												let y = rnd.Next(25, 365 * 25)
												select new EmployeeEntity()
												{
													FirstName = x[0],
													LastName = x[1],
													Active = (rnd.Next(0, 1000) % 2) == 0,
													Compensation = rnd.Next(10000, 200000),
													StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(y)).Date,
													LastPromtion = (rnd.Next(0, 1000) % 2) == 0 ? DateTime.Now.Subtract(TimeSpan.FromDays(y > (365 * 2) ? rnd.Next(25, 365 * 2) : (int)(y * .85))).Date : null,
													JobTitle = "Original Title",
													RecentWarnings = (rnd.Next(0, 1000) % 2) == 0
												};

			//
			// Save the list to the workflow context.
			//
			context.Properties.Set(WellKnown.Context.Data, items);
			returnValue = true;

			return Task.FromResult(returnValue);
		}
	}
}

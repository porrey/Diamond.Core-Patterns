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
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// Display all promoted employees.
	/// </summary>
	public class SampleWorkStep5 : WorkflowItemTemplate
	{
		public SampleWorkStep5(ILogger<SampleWorkStep5> logger, IRepositoryFactory repositoryFactory)
			: base(logger)
		{
			this.RepositoryFactory = repositoryFactory;
		}

		public override string Name => $"Sample Step {this.Ordinal}";
		public override string Group => WellKnown.Workflow.SampleWorkflow;
		public override int Ordinal => 5;
		protected IRepositoryFactory RepositoryFactory { get; set; }

		protected override async Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Get a repository for IEmployeeEntity.
			//(
			IReadOnlyRepository<IEmployeeEntity> repository = await this.RepositoryFactory.GetReadOnlyAsync<IEmployeeEntity>();

			//
			// Get the promoted employees.
			//
			IEnumerable<IEmployeeEntity> promotedEmployees = await repository.GetAsync(t => t.JobTitle == "Promoted");

			//
			// Display the employee.
			//
			this.Logger.LogInformation("These employees have been promoted:");
			foreach (IEmployeeEntity employee in promotedEmployees)
			{
				this.Logger.LogInformation("{employee}", employee);
			}

			//
			// Since we are using transient lifetimes, we need to dispose.
			//
			await repository.TryDisposeAsync();

			returnValue = true;

			return returnValue;
		}
	}
}

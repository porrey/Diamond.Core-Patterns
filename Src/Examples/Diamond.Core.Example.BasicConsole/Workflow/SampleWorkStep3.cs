//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Diamond.Core.UnitOfWork;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// This step will populate the database with random employees.
	/// </summary>
	public class SampleWorkStep3 : WorkflowItem
	{
		public SampleWorkStep3(ILogger<SampleWorkStep3> logger, IRepositoryFactory repositoryFactory, IUnitOfWorkFactory unitOfWorkFactory)
			: base(logger)
		{
			this.RepositoryFactory = repositoryFactory;
			this.UnitOfWorkFactory = unitOfWorkFactory;
		}

		public override string Name => $"Sample Step {this.Ordinal}";
		public override string Group => WellKnown.Workflow.SampleWorkflow;
		public override int Ordinal => 3;
		protected IRepositoryFactory RepositoryFactory { get; set; }
		protected IUnitOfWorkFactory UnitOfWorkFactory { get; set; }

		public override async Task<bool> OnShouldExecuteAsync(IContext context)
		{
			//
			// Get a repository for IEmployeeEntity.
			//
			IQueryableRepository<IEmployeeEntity> repository = await this.RepositoryFactory.GetQueryableAsync<IEmployeeEntity>();

			//
			// Get the context.
			//
			IRepositoryContext db = await repository.GetContextAsync();

			return await db.CanConnectAsync();
		}

		protected override async Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Get the data from the context.
			//
			IEnumerable<IEmployeeEntity> items = context.Properties.Get<IEnumerable<IEmployeeEntity>>(WellKnown.Context.Data);

			//
			// Get a repository for IEmployeeEntity.
			//
			IUnitOfWork<(bool, int?), IEmployeeEntity> uow = await this.UnitOfWorkFactory.GetAsync<(bool, int?), IEmployeeEntity>(WellKnown.UnitOfWork.CreateEmployee);

			//
			// Track errors.
			//
			int errorCount = 0;

			//
			// Add the employees to the database.
			//
			foreach (var item in items)
			{
				try
				{
					(bool result, int? id) = await uow.CommitAsync(item);

					if (result)
					{
						this.Logger.LogInformation("The employee {firstName} {lastName} was successfully created wit Employee ID {employeeId}.", item.FirstName, item.LastName, item.Id);
					}
					else
					{
						this.Logger.LogError("The employee {firstName} {lastName} was not created", item.FirstName, item.LastName);
						errorCount++;
					}
				}
				catch (Exception ex)
				{
					this.Logger.LogError(ex, "Exception while creating the employee {firstName} {lastName}", item.FirstName, item.LastName);
					errorCount++;
				}
			}

			//
			// Check the result.
			//
			if (errorCount == 0)
			{
				this.Logger.LogInformation("Successfully created {count} employees.", items.Count());
				returnValue = true;
			}
			else
			{
				await this.StepFailedAsync(context, $"{errorCount} employees out ArgumentOutOfRangeException {items.Count()} StepFailedAsync to created.");
			}

			return returnValue;
		}
	}
}

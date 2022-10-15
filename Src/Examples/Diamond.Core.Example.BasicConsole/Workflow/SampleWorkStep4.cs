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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Decorator;
using Diamond.Core.Specification;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.BasicConsole
{
	public class SampleWorkStep4 : WorkflowItemTemplate
	{
		public SampleWorkStep4(ILogger<SampleWorkStep4> logger, ISpecificationFactory specificationFactory, IDecoratorFactory decoratorFactory)
			: base(logger)
		{
			this.SpecificationFactory = specificationFactory;
			this.DecoratorFactory = decoratorFactory;
		}

		public override string Name => $"Sample Step {this.Ordinal}";
		public override string Group => WellKnown.Workflow.SampleWorkflow;
		public override int Ordinal => 4;
		protected IDecoratorFactory DecoratorFactory { get; set; }
		protected ISpecificationFactory SpecificationFactory { get; set; }

		protected override async Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Get the specification to retrieve all active employees.
			//
			ISpecification<IEnumerable<int>> activeEmployeeSpecification = await this.SpecificationFactory.GetAsync<IEnumerable<int>>(WellKnown.Specifcation.GetActiveEmployeeIdList);

			//
			// Get the list.
			//
			IEnumerable<int> activeEmployees = await activeEmployeeSpecification.ExecuteSelectionAsync();

			if (activeEmployees.Any())
			{
				//
				// Get the specification to retrieve employee details.
				//
				ISpecification<int, IEmployeeEntity> employeeDetailsSpecification = await this.SpecificationFactory.GetAsync<int, IEmployeeEntity>(WellKnown.Specifcation.GetEmployeeDetails);

				//
				// Promote all eligible employees.
				//
				foreach (int activeEmployeeId in activeEmployees)
				{
					//
					// Get the employee details.
					//
					IEmployeeEntity employee = await employeeDetailsSpecification.ExecuteSelectionAsync(activeEmployeeId);

					//
					// Get the promotion decorator.
					//
					IDecorator<IEmployeeEntity, (bool, IEmployeeEntity, string)> decorator = await this.DecoratorFactory.GetAsync<IEmployeeEntity, (bool, IEmployeeEntity, string)>(WellKnown.Decorator.EmployeePromotion, employee);

					//
					// Attempt to promote the employee.
					//
					(bool promotionResult, IEmployeeEntity updatedEmployee, string message) = await decorator.TakeActionAsync();

					//
					// Check the result.
					//
					if (promotionResult)
					{
						this.Logger.LogInformation("The employee {firstName} {lastName} was promoted from {previousTitle} to {newTitle} with a salary increase from {previousSalary} to {newSalary}.",
													employee.FirstName, employee.LastName, employee.JobTitle, updatedEmployee.JobTitle, employee.Compensation.ToString("$#,###"), updatedEmployee.Compensation.ToString("$#,###"));
					}
					else
					{
						this.Logger.LogInformation("The employee {firstName} {lastName} was not eligible for a promotion: '{message}'.", employee.FirstName, employee.LastName, message);
					}

					//
					// Dispose the decorator.
					//
					await decorator.TryDisposeAsync();
				}

				//
				// Since we are using transient lifetimes, we need to dispose.
				//
				await employeeDetailsSpecification.TryDisposeAsync();

				returnValue = true;
			}
			else
			{
				this.Logger.LogWarning("There are no active employees in the company.");
				returnValue = true;
			}

			//
			// Since we are using transient lifetimes, we need to dispose.
			//
			await activeEmployeeSpecification.TryDisposeAsync();

			return returnValue;
		}
	}
}

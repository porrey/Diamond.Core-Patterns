using System;
using System.Threading.Tasks;
using Diamond.Core.Decorator;
using Diamond.Core.Rules;
using Diamond.Core.UnitOfWork;

namespace Diamond.Core.Example.BasicConsole
{
	public class EmployeePromotionDecorator : IDecorator<IEmployeeEntity, (bool, IEmployeeEntity, string)>
	{
		public EmployeePromotionDecorator(IUnitOfWorkFactory unitOfWorkFactory, IRulesFactory rulesFactory)
		{
			this.UnitOfWorkFactory = unitOfWorkFactory;
			this.RulesFactory = rulesFactory;
		}

		public string Name => "EmployeePromotion";
		protected IUnitOfWorkFactory UnitOfWorkFactory { get; set; }
		protected IRulesFactory RulesFactory { get; set; }

		public async Task<(bool, IEmployeeEntity, string)> TakeActionAsync(IEmployeeEntity item)
		{
			//
			// Clone the employee to preserve the previous data.
			//
			(bool result, IEmployeeEntity updatedEntity, string message) = (false, null, null);

			//
			// Evaluate the employee for a promotion.
			//
			message = await this.RulesFactory.EvaluateAsync(WellKnown.Rules.EmployeePromotion, item);

			if (String.IsNullOrWhiteSpace(message))
			{
				//
				// Get the Unit of Work to promote the employee.
				//
				IUnitOfWork<(bool, IEmployeeEntity), (int employeeId, string newTitle, decimal percentRaise)> uow = await this.UnitOfWorkFactory.GetAsync<(bool, IEmployeeEntity), (int, string, decimal)>(WellKnown.UnitOfWork.PromoteEmployee);

				//
				// Promote the employee with a 10% raise and a new title.
				//
				(result, updatedEntity) = await uow.CommitAsync((item.Id, "Promoted", .1M));

				//
				// Check the result.
				//
				if (!result)
				{
					throw new Exception($"The employee with ID {item.Id} could not be updated.");
				}
			}

			return (result, updatedEntity, message);
		}
	}
}

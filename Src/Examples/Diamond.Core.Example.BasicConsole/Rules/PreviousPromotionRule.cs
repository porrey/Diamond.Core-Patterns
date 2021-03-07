using System;
using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// The employee not have been promoted with 60 days.
	/// </summary>
	public class PreviousPromotionRule : IRule<IEmployeeEntity>
	{
		public string Group => WellKnown.Rules.EmployeePromotion;

		public Task<IRuleResult> ValidateAsync(IEmployeeEntity item)
		{
			IRuleResult returnValue = new RuleResult();

			if (!item.LastPromtion.HasValue || (item.LastPromtion.HasValue && DateTime.Now.Subtract(item.LastPromtion.Value).TotalDays > 60))
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = "The employee has been promoted within the last year.";
			}

			return Task.FromResult(returnValue);
		}
	}
}

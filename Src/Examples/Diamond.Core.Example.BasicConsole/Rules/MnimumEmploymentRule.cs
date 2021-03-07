using System;
using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// The employee must have been with the company at least 30 days.
	/// </summary>
	public class MnimumEmploymentRule : IRule<IEmployeeEntity>
	{
		public string Group => WellKnown.Rules.EmployeePromotion;

		public Task<IRuleResult> ValidateAsync(IEmployeeEntity item)
		{
			IRuleResult returnValue = new RuleResult();

			if (DateTime.Now.Subtract(item.StartDate).TotalDays >= 30)
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = "The employee has been with the company less than 30 days.";
			}

			return Task.FromResult(returnValue);
		}
	}
}

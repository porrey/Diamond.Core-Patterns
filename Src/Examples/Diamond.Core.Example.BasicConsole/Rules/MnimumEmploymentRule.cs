using System;
using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// The employee must have been with the company at least 30 days.
	/// </summary>
	public class MnimumEmploymentRule : RuleTemplate<IEmployeeEntity>
	{
		public MnimumEmploymentRule()
			: base(WellKnown.Rules.EmployeePromotion)
		{
		}

		protected override Task<IRuleResult> OnValidateAsync(IEmployeeEntity item)
		{
			IRuleResult returnValue = new RuleResultTemplate();

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

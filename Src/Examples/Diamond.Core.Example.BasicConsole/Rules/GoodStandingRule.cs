using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// The employee must not have any recent warnings.
	/// </summary>
	public class GoodStandingRule : IRule<IEmployeeEntity>
	{
		public string Group => WellKnown.Rules.EmployeePromotion;

		public Task<IRuleResult> ValidateAsync(IEmployeeEntity item)
		{
			IRuleResult returnValue = new RuleResult();

			if (!item.RecentWarnings)
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = "The employee has recent warnings.";
			}

			return Task.FromResult(returnValue);
		}
	}
}

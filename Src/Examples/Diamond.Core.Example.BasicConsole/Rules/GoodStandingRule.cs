using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// The employee must not have any recent warnings.
	/// </summary>
	public class GoodStandingRule : RuleTemplate<IEmployeeEntity>
	{
		public GoodStandingRule()
			: base(WellKnown.Rules.EmployeePromotion)
		{
		}

		protected override Task<IRuleResult> OnValidateAsync(IEmployeeEntity item)
		{
			IRuleResult returnValue = new RuleResultTemplate();

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

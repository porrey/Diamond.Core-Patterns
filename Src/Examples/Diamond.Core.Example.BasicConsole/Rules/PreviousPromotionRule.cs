using System;
using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// The employee not have been promoted with 60 days.
	/// </summary>
	public class PreviousPromotionRule : RuleTemplate<IEmployeeEntity>
	{
		public PreviousPromotionRule()
			: base(WellKnown.Rules.EmployeePromotion)
		{
		}

		protected override Task<IRuleResult> OnValidateAsync(IEmployeeEntity item)
		{
			IRuleResult returnValue = new RuleResultTemplate();

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

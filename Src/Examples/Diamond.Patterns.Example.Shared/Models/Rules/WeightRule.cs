using System.Threading.Tasks;
using Diamond.Patterns.Rules;

namespace Diamond.Patterns.Example
{
	public class WeightRule : IRule<IShipmentModel>
	{
		public string Group { get; set; } = WellKnown.Rules.Shipment;

		public Task<IRuleResult> ValidateAsync(IShipmentModel item)
		{
			IRuleResult returnValue = new RuleResult();

			if (item.Weight > 0)
			{
				returnValue.Passed = true;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage=$"The shipment '{ item.ProNumber}' has an invalid weight.";
			}

			return Task.FromResult(returnValue);
		}
	}
}

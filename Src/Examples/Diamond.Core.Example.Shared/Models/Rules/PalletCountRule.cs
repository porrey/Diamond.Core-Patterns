using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example
{
	public class PalletCountRule : IRule<IShipmentModel>
	{
		public string Group { get; set; } = WellKnown.Rules.Shipment;

		public Task<IRuleResult> ValidateAsync(IShipmentModel item)
		{
			IRuleResult returnValue = new RuleResult();

			if (item.PalletCount > 0)
			{
				returnValue.Passed = true;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage=$"The shipment '{ item.ProNumber}' has an invalid pallet count.";
			}

			return Task.FromResult(returnValue);
		}
	}
}

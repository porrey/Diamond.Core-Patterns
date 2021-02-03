using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example
{
	public class DeliveryAddressRule : IRule<IShipmentModel>
	{
		public string Group { get; set; } = WellKnown.Rules.Shipment;

		public Task<IRuleResult> ValidateAsync(IShipmentModel item)
		{
			IRuleResult returnValue = new RuleResult();

			if (!string.IsNullOrWhiteSpace(item.DeliveryAddress))
			{
				returnValue.Passed = true;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = $"The shipment '{ item.ProNumber}' must have a delivery address.";
			}

			return Task.FromResult(returnValue);
		}
	}
}

﻿using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example {
	public class PickupAddressRule : IRule<IShipmentModel> {
		public string Group { get; set; } = WellKnown.Rules.Shipment;

		public Task<IRuleResult> ValidateAsync(IShipmentModel item) {
			IRuleResult returnValue = new RuleResult();

			if (!string.IsNullOrWhiteSpace(item.PickupAddress)) {
				returnValue.Passed = true;
			}
			else {
				returnValue.Passed = false;
				returnValue.ErrorMessage = $"The shipment '{ item.ProNumber}' must have a pickup address.";
			}

			return Task.FromResult(returnValue);
		}
	}
}

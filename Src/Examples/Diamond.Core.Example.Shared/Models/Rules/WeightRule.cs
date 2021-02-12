//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System.Threading.Tasks;
using Diamond.Core.Rules;

namespace Diamond.Core.Example
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
				returnValue.ErrorMessage = $"The shipment '{ item.ProNumber}' has an invalid weight.";
			}

			return Task.FromResult(returnValue);
		}
	}
}

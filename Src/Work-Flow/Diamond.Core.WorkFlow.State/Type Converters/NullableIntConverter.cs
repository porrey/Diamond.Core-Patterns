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
using System;
using System.Threading.Tasks;

namespace Diamond.Core.WorkFlow.State {
	/// <summary>
	/// 
	/// </summary>
	public class NullableIntConverter : ConverterBase<int?> {
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override (bool, string, object) OnConvertSource() {
			(bool Success, string ErrorMessage, int? ConvertedValue) returnValue = (false, null, null);

			if (String.IsNullOrWhiteSpace(this.SourceStringValue)) {
				returnValue.Success = true;
				returnValue.ConvertedValue = null;
			}
			else {
				if (Int32.TryParse(this.SourceStringValue, out int result)) {
					returnValue.Success = true;
					;
					returnValue.ConvertedValue = result;
				}
				else {
					returnValue.ErrorMessage = $"The value '{this.SourceObjectValue}' cannot be converted to an integer.";
				}
			}

			return returnValue;
		}
	}
}

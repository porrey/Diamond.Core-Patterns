// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;

namespace Diamond.Patterns.WorkFlow.State
{
	public class TimeSpanConverter : ConverterBase<TimeSpan>
	{
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, TimeSpan ConvertedValue) returnValue = (false, null, TimeSpan.Zero);

			if (!String.IsNullOrWhiteSpace(this.SourceStringValue))
			{
				if (TimeSpan.TryParse(this.SourceStringValue, out TimeSpan value))
				{
					returnValue.ConvertedValue = value;
					returnValue.Success = true;
				}
				else
				{
					returnValue.ErrorMessage = $"The value '{this.SourceObjectValue}' cannot be converted to a Time Span.";
				}				
			}
			else
			{
				returnValue.ErrorMessage = "Cannot convert empty string or space(s) to a Time Span value.";
			}

			return returnValue;
		}
	}
}

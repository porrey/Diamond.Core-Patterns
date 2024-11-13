//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
using System.ComponentModel;

namespace Diamond.Core.Workflow.State
{
	/// <summary>
	/// 
	/// </summary>
	public class DateConverter : ConverterBase<DateTime>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, DateTime ConvertedValue) returnValue = (false, null, DateTime.MinValue);

			if (!String.IsNullOrWhiteSpace(this.SourceStringValue))
			{
				//
				// Get the converter for string
				//
				TypeConverter converter = TypeDescriptor.GetConverter(this.TargetType);

				//
				// Allow the word NOW to indicate the current date and time.
				//
				if (this.SourceStringValue.ToLower().Equals("now"))
				{
					returnValue.ConvertedValue = DateTime.Now;
					returnValue.Success = true;
				}
				else if (converter.CanConvertFrom(typeof(string)))
				{
					returnValue.ConvertedValue = (DateTime)converter.ConvertFromString(this.SourceStringValue);
					returnValue.Success = true;
				}
			}
			else
			{
				returnValue.ErrorMessage = "Cannot convert empty string or space(s) to a Date Time value.";
			}

			return returnValue;
		}
	}
}

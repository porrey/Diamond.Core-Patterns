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
using System.Linq;

namespace Diamond.Core.Workflow.State
{
	/// <summary>
	/// 
	/// </summary>
	public class EnumConverter : ConverterBase<Enum>
	{
		/// <summary>
		/// 
		/// </summary>
		public EnumConverter()
			: this(new char[] { '|', ',', ';' })
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="separator"></param>
		public EnumConverter(char[] separator)
		{
			this.Separator = separator;
		}

		/// <summary>
		/// 
		/// </summary>
		protected char[] Separator { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, object ConvertedValue) returnValue = (false, null, null);

			string localValue = this.SourceStringValue;

			if (String.IsNullOrWhiteSpace(this.SourceStringValue))
			{
				//
				// Use 'None' for blanks. If the enumeration has not defined 'None' then the
				// conversion will fail as expected.
				//
				localValue = "none";
			}

			//
			// Check if this enumeration type has the Flags attribute.
			//
			bool multi = this.SpecificTargetType.CustomAttributes.Where(t => t.AttributeType == typeof(FlagsAttribute)).Count() > 0;

			if (multi)
			{
				//
				// This type supports multiple values.
				//
				string[] items = localValue.Split(this.Separator, StringSplitOptions.RemoveEmptyEntries);
				int values = 0;

				foreach (string item in items)
				{
					try
					{
						values += Convert.ToInt32(Enum.Parse(this.SpecificTargetType, item, true));
					}
					catch
					{
						returnValue.ErrorMessage = $"The value '{item}' is not valid for the type '{this.SpecificTargetType.Name}'.";
					}
				}

				returnValue.ConvertedValue = values;
				returnValue.Success = String.IsNullOrWhiteSpace(returnValue.ErrorMessage);
			}
			else
			{
				//
				// Check if the value supplied has multiple values.
				//
				int pos = localValue.IndexOfAny(this.Separator);

				if (localValue.IndexOfAny(this.Separator) == -1)
				{
					try
					{
						returnValue.ConvertedValue = Enum.Parse(this.SpecificTargetType, localValue, true);
						returnValue.Success = true;
					}
					catch
					{
						returnValue.ErrorMessage = $"The value '{localValue}' is not valid for type '{this.SpecificTargetType.Name}'.";
					}
				}
				else
				{
					returnValue.ErrorMessage = $"The type '{this.SpecificTargetType.Name}' does not support multiple values.";
				}
			}


			return returnValue;
		}
	}
}

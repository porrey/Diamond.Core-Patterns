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

namespace Diamond.Patterns.StateDictionary
{
	public class StringArrayConverter : ConverterBase<string[]>
	{
		public StringArrayConverter()
			: this(new char[] { '|', ',', ';' })
		{
		}

		public StringArrayConverter(char[] separator)
		{
			this.Separator = separator;
		}

		protected char[] Separator { get; set; }

		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, string[] ConvertedValue) returnValue = (false, null, new string[0]);

			returnValue.ConvertedValue = this.SourceStringValue.Split(this.Separator, StringSplitOptions.RemoveEmptyEntries);
			returnValue.Success = true;

			return returnValue;
		}
	}
}

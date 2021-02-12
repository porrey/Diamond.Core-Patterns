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
using System.Collections.Generic;

namespace Diamond.Core.WorkFlow.State
{
	/// <summary>
	/// 
	/// </summary>
	public class IDictionaryConverter : ConverterBase<IDictionary<string, string>>
	{
		/// <summary>
		/// 
		/// </summary>
		public IDictionaryConverter()
			: this(new char[] { '|', ',', ';' })
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="separator"></param>
		public IDictionaryConverter(char[] separator)
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
			(bool success, string errorMessage, IDictionary<string, string> convertedValue) = (false, null, new Dictionary<string, string>());

			string[] pairs = this.SourceStringValue.Split(this.Separator, StringSplitOptions.RemoveEmptyEntries);

			foreach (string pair in pairs)
			{
				int position = pair.IndexOf("=");

				if (position > 0)
				{
					string key = pair.Substring(0, position);
					string value = pair.Substring(position + 1, pair.Length - position - 1);
					convertedValue.Add(key, value);
				}
			}

			success = true;

			return (success, errorMessage, convertedValue);
		}
	}
}

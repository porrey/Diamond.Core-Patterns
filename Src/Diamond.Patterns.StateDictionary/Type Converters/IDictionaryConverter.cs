using System;
using System.Collections.Generic;

namespace Diamond.Patterns.StateDictionary
{
	public class IDictionaryConverter : ConverterBase<IDictionary<string, string>>
	{
		public IDictionaryConverter()
			: this(new char[] { '|', ',', ';' })
		{
		}

		public IDictionaryConverter(char[] separator)
		{
			this.Separator = separator;
		}

		protected char[] Separator { get; set; }

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

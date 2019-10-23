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

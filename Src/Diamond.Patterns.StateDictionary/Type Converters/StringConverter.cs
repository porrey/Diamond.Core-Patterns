using System;

namespace Diamond.Patterns.StateDictionary
{
	public class StringConverter : ConverterBase<string>
	{
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, string ConvertedValue) returnValue = (false, null, String.Empty);

			returnValue.ConvertedValue = this.SourceStringValue;
			returnValue.Success = true;

			return returnValue;
		}
	}
}

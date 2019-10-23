using System;

namespace Diamond.Patterns.StateDictionary
{
	public class NullableDateConverter : ConverterBase<DateTime?>
	{
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, DateTime? ConvertedValue) returnValue = (false, null, null);

			

			return returnValue;
		}
	}
}

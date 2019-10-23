using System;
using System.ComponentModel;

namespace Diamond.Patterns.StateDictionary
{
	public class DateConverter : ConverterBase<DateTime>
	{
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, DateTime ConvertedValue) returnValue = (false, null, DateTime.MinValue);

			if (!String.IsNullOrWhiteSpace(this.SourceStringValue))
			{
				// ***
				// *** Get the converter for string
				// ***
				TypeConverter converter = TypeDescriptor.GetConverter(this.TargetType);

				// ***
				// *** Allow the word NOW to indicate the current date and time.
				// ***
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

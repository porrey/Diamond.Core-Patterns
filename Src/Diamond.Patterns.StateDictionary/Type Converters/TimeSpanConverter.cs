using System;

namespace Diamond.Patterns.StateDictionary
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

using System;

namespace Diamond.Patterns.StateDictionary
{
	public class DoubleConverter : ConverterBase<Double>
	{
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, Double ConvertedValue) returnValue = (false, null, 0.0F);

			if (String.IsNullOrWhiteSpace(this.SourceStringValue))
			{
				returnValue.ErrorMessage = $"Cannot convert empty string or space(s) to a double.";
			}
			else
			{
				if (Double.TryParse(this.SourceStringValue, out Double result))
				{
					returnValue.Success = true;
					returnValue.ConvertedValue = result;
				}
				else
				{
					returnValue.ErrorMessage = $"The value '{this.SourceObjectValue}' cannot be converted to a double.";
				}
			}

			return returnValue;
		}
	}
}

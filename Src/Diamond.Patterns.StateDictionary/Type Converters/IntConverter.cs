using System;

namespace Diamond.Patterns.StateDictionary
{
	public class IntConverter : ConverterBase<int>
	{
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, int ConvertedValue) returnValue = (false, null, 0);

			if (String.IsNullOrWhiteSpace(this.SourceStringValue))
			{
				returnValue.ErrorMessage = $"Cannot convert empty string or space(s) to an integer.";
			}
			else
			{
				if (Int32.TryParse(this.SourceStringValue, out int result))
				{
					returnValue.Success = true;
					returnValue.ConvertedValue = result;
				}
				else
				{
					returnValue.ErrorMessage = $"The value '{this.SourceObjectValue}' cannot be converted to an integer.";
				}
			}

			return returnValue;
		}
	}
}

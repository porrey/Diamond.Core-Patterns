using System;

namespace Diamond.Patterns.StateDictionary
{
	public class UintConverter : ConverterBase<uint>
	{
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, uint ConvertedValue) returnValue = (false, null, 0);

			if (String.IsNullOrWhiteSpace(this.SourceStringValue))
			{
				returnValue.ErrorMessage = $"Cannot convert empty string or space(s) to an unsigned integer.";
			}
			else
			{
				if (UInt32.TryParse(this.SourceStringValue, out uint result))
				{
					returnValue.Success = true;
					returnValue.ConvertedValue = result;
				}
				else
				{
					returnValue.ErrorMessage = $"The value '{this.SourceObjectValue}' cannot be converted to an unsigned integer.";
				}
			}

			return returnValue;
		}
	}
}

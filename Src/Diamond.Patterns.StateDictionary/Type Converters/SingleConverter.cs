using System;

namespace Diamond.Patterns.StateDictionary
{
	public class SingleConverter : ConverterBase<Single>
	{
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, Single ConvertedValue) returnValue = (false, null, 0.0F);

			if (String.IsNullOrWhiteSpace(this.SourceStringValue))
			{
				returnValue.ErrorMessage = $"Cannot convert empty string or space(s) to a float.";
			}
			else
			{
				if (Single.TryParse(this.SourceStringValue, out Single result))
				{
					returnValue.Success = true;
					returnValue.ConvertedValue = result;
				}
				else
				{
					returnValue.ErrorMessage = $"The value '{this.SourceObjectValue}' cannot be converted to a float.";
				}
			}

			return returnValue;
		}
	}
}

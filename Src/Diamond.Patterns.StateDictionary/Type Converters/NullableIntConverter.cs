using System;
using System.Threading.Tasks;

namespace Diamond.Patterns.StateDictionary
{
	public class NullableIntConverter : ConverterBase<int?>
	{
		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, int? ConvertedValue) returnValue = (false, null, null);

			if (String.IsNullOrWhiteSpace(this.SourceStringValue))
			{
				returnValue.Success = true;
				returnValue.ConvertedValue = null;
			}
			else
			{
				if (Int32.TryParse(this.SourceStringValue, out int result))
				{
					returnValue.Success = true;;
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

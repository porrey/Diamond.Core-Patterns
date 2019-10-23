using System.Linq;

namespace Diamond.Patterns.StateDictionary
{
	public class BoolConverter : ConverterBase<bool>
	{
		protected string[] SupportedTrueValues = new string[] { "yes", "true", "1", "on"};

		protected override (bool, string, object) OnConvertSource()
		{
			(bool Success, string ErrorMessage, bool ConvertedValue) returnValue = (false, null, false);

			if (this.SupportedTrueValues.Contains(this.SourceStringValue.ToLower()))
			{
				returnValue.ConvertedValue = true;
				returnValue.Success = true;
			}
			else
			{
				returnValue.ConvertedValue = false;
				returnValue.Success = true;
			}

			return returnValue;
		}
	}
}

using System;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.StateDictionary
{
	public class ConverterBase<TTargetType> : IStateTypeConverter
	{
		public Type TargetType => typeof(TTargetType);

		protected string SourceStringValue { get; set; }
		protected object SourceObjectValue { get; set; }

		public Type SpecificTargetType { get; set; }

		public virtual (bool, string, object) ConvertSource(object sourceValue, Type specificTargetType)
		{
			this.SourceObjectValue = sourceValue;
			this.SourceStringValue = Convert.ToString(sourceValue);
			this.SpecificTargetType = specificTargetType;
			return this.OnConvertSource();
		}

		protected virtual (bool, string, object) OnConvertSource()
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return this.TargetType.Name;
		}
	}
}

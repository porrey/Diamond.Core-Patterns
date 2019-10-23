using System;

namespace Diamond.Patterns.Abstractions
{
	public interface IStateTypeConverter
	{
		Type TargetType { get; }
		(bool, string, object) ConvertSource(object sourceValue, Type specificTargetType);
	}
}

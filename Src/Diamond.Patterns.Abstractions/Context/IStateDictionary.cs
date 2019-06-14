using System;
using System.Collections.Generic;

namespace Diamond.Patterns.Abstractions
{
	public interface IStateDictionary : IDictionary<string, object>
	{
		bool CanConvertParameter(string key, Type targetType, out object convertedValue);
		bool CanConvertParameter<T>(string key, out T convertedValue);
		T Get<T>(string key, T defaultValue = default);
		TProperty Get<TProperty>(string key);
		void Set<TProperty>(string key, TProperty value);
		TProperty TryGet<TProperty>(string key, TProperty initializeValue);
	}
}
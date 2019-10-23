using System;
using System.Collections.Generic;

namespace Diamond.Patterns.Abstractions
{
	public interface IStateDictionary : IDictionary<string, object>
	{
		(bool, string, object) ConvertParameter(string key, Type targetType);
		(bool, string, T) ConvertParameter<T>(string key);
		TProperty Get<TProperty>(string key);
		TProperty Get<TProperty>(string key, TProperty defaultValue = default(TProperty));
		TProperty TryGet<TProperty>(string key, TProperty initializeValue);
		void Set<TProperty>(string key, TProperty value);
	}
}
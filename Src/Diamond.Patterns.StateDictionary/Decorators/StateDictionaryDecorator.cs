using System.Collections.Generic;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.StateDictionary
{
	public static class StateDictionaryDecorator
	{
		public static Task Merge(this IStateDictionary target, IStateDictionary source)
		{
			// ***
			// *** Add the iteration instance context properties
			// *** to the current context.
			// ***
			foreach (KeyValuePair<string, object> property in source)
			{
				if (target.ContainsKey(property.Key))
				{
					target[property.Key] = property.Value;
				}
				else
				{
					target.Add(property.Key, property.Value);
				}
			}

			return Task.FromResult(0);
		}

		public static Task Remove(this IStateDictionary target, IStateDictionary source)
		{
			// ***
			// *** Add the iteration instance context properties
			// *** to the current context.
			// ***
			foreach (KeyValuePair<string, object> property in source)
			{
				if (target.ContainsKey(property.Key))
				{
					target.Remove(property.Key);
				}
			}

			return Task.FromResult(0);
		}
	}
}

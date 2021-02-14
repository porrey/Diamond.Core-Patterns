//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
// 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diamond.Core.WorkFlow.State
{
	/// <summary>
	/// 
	/// </summary>
	public static class StateDictionaryDecorator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public static Task Merge(this IStateDictionary target, IStateDictionary source)
		{
			//
			// Add the iteration instance context properties
			// to the current context.
			//
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public static Task Remove(this IStateDictionary target, IStateDictionary source)
		{
			//
			// Add the iteration instance context properties
			// to the current context.
			//
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

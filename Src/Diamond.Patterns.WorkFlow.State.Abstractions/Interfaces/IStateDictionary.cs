// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using System.Collections.Generic;

namespace Diamond.Patterns.WorkFlow.State
{
	/// <summary>
	/// Defines a dictionary that can be used to manage state in work flows
	/// or other patterns.
	/// </summary>
	public interface IStateDictionary : IDictionary<string, object>
	{
		/// <summary>
		/// Retrieves and converts a dictionary item to the specified type.
		/// </summary>
		/// <param name="key">The unique key of the item to retrieve.</param>
		/// <param name="targetType">The target type.</param>
		/// <returns>The value of the dictionary item in the specified type or an 
		/// error if the item could not be converted.</returns>
		(bool, string, object) ConvertParameter(string key, Type targetType);

		/// <summary>
		/// Retrieves and converts a dictionary item to the specified type.
		/// </summary>
		/// <typeparam name="TTarget">The target type.</typeparam>
		/// <param name="key">The unique key of the item to retrieve.</param>
		/// <returns>The value of the dictionary item in the specified type or an 
		/// error if the item could not be converted.</returns>
		(bool, string, TTarget) ConvertParameter<TTarget>(string key);

		/// <summary>
		/// Retrieves a dictionary item and  casts it to the specified type. No 
		/// conversion is performed. An exception is thrown if the key is invalid.
		/// </summary>
		/// <typeparam name="TProperty">The target type.</typeparam>
		/// <param name="key">The unique key of the item to retrieve.</param>
		/// <returns>returns the item as the given type.</returns>
		TProperty Get<TProperty>(string key);

		/// <summary>
		/// Retrieves a dictionary item and  casts it to the specified type. No 
		/// conversion is performed. The default value is returned if the key
		/// is invalid.
		/// </summary>
		/// <typeparam name="TProperty">The target type.</typeparam>
		/// <param name="key">The unique key of the item to retrieve.</param>
		/// <param name="defaultValue">The default value to return if the item does not exist.</param>
		/// <returns>returns the item (or the default value) as the given type.</returns>
		TProperty Get<TProperty>(string key, TProperty defaultValue = default(TProperty));

		/// <summary>
		/// Attempts to retrieve an item from the dictionary by the specified key. If the
		/// item does not exist it will be created and initialized with initializeValue.
		/// </summary>
		/// <typeparam name="TProperty">The target type.</typeparam>
		/// <param name="key">The unique key of the item to retrieve.</param>
		/// <param name="initializeValue">The value to initialize the item to if it does not exist.</param>
		/// <returns>Returns the existing item or the initialized item.</returns>
		TProperty TryGet<TProperty>(string key, TProperty initializeValue);

		/// <summary>
		/// Sets a value in the dictionary. I the item does not exist, it is created. If
		/// the item already exists, it is updated. Attempting to change the item type
		/// could have unexpected side affects and is not recommended.
		/// </summary>
		/// <typeparam name="TProperty">The target type.</typeparam>
		/// <param name="key">The unique key of the item to retrieve.</param>
		/// <param name="value">The value used to set the item.</param>
		void Set<TProperty>(string key, TProperty value);
	}
}
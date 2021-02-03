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
using System.Threading.Tasks;

namespace Diamond.Core.Abstractions
{
	/// <summary>
	/// Defines a generic object factory that can be wrapped around a container to
	/// decouple the application from a specific container.
	/// </summary>
	public interface IObjectFactory
	{
		/// <summary>
		/// Gets an instance of an object by type.
		/// </summary>
		/// <typeparam name="TService">The type of the object to retrieve.</typeparam>
		/// <param name="skipInitialization">Set to true if the object implements <see cref="IRequiresInitialization"/> but
		/// the initialization should be skipped.</param>
		/// <returns>The instance of the specified object.</returns>
		TService GetInstance<TService>(bool skipInitialization = false);

		/// <summary>
		/// Gets an instance of an object by type and name.
		/// </summary>
		/// <typeparam name="TService">The type of the object to retrieve.</typeparam>
		/// <param name="name">The name of the object to retrieve.</param>
		/// <param name="skipInitialization"></param>
		/// <returns>The instance of the specified object.</returns>
		TService GetInstance<TService>(string name, bool skipInitialization = false);

		/// <summary>
		/// Gets an instance of an object by type.
		/// </summary>
		/// <param name="objectType">The type of the object to retrieve.</param>
		/// <param name="skipInitialization">Set to true if the object implements <see cref="IRequiresInitialization"/> but
		/// the initialization should be skipped.</param>
		/// <returns>The instance of the specified object.</returns>
		object GetInstance(Type objectType, bool skipInitialization = false);

		/// <summary>
		/// Gets an instance of an object by type.
		/// </summary>
		/// <param name="objectType"></param>
		/// <param name="name">The name of the object to retrieve.</param>
		/// <param name="skipInitialization">Set to true if the object implements <see cref="IRequiresInitialization"/> but
		/// the initialization should be skipped.</param>
		/// <returns>The instance of the specified object.</returns>
		object GetInstance(Type objectType, string name, bool skipInitialization = false);

		/// <summary>
		/// Get all instances by type.
		/// </summary>
		/// <typeparam name="TService">The type of object to retrieve.</typeparam>
		/// <returns>The all instances of the specified object.</returns>
		IEnumerable<TService> GetAllInstances<TService>();

		/// <summary>
		/// Get all instances by type.
		/// </summary>
		/// <param name="objectType">The type of object to retrieve.</param>
		/// <returns>The all instances of the specified object.</returns>
		IEnumerable<object> GetAllInstances(Type objectType);

		/// <summary>
		/// Gets an object that implements a specific interfaces, not necessarily the interface
		/// registered in the container.
		/// </summary>
		/// <typeparam name="TService">The type of object to retrieve.</typeparam>
		/// <returns></returns>
		Task<IList<TService>> ResolveByInterfaceAsync<TService>();

		/// <summary>
		/// Registers an instance of an object with the specified name.
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <param name="name"></param>
		/// <param name="instance"></param>
		void RegisterSingletonInstance<TService>(string name, TService instance);

		/// <summary>
		/// Checks if an object implements <see cref="IRequiresInitialization"/> and
		/// initializes it if necessary.
		/// </summary>
		/// <param name="item">The instance to check and in initialize.</param>
		/// <returns></returns>
		Task<bool> InitializeIfRequiredAsync(object item);
	}
}

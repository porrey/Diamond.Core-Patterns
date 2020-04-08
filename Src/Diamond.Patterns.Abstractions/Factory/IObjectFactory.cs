// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
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

namespace Diamond.Patterns.Abstractions
{
	public interface IObjectFactory
	{
		TService GetInstance<TService>(bool skipInitialization = false);
		TService GetInstance<TService>(string name, bool skipInitialization = false);
		object GetInstance(Type objectType, bool skipInitialization = false);
		object GetInstance(Type objectType, string name, bool skipInitialization = false);
		IEnumerable<TService> GetAllInstances<TService>();
		IEnumerable<object> GetAllInstances(Type objectType);
		Task<IList<T>> ResolveByInterfaceAsync<T>();
		void RegisterSingletonInstance<T>(string name, T instance);
		Task<bool> InitializeIfRequiredAsync(object item);
	}
}

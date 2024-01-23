//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
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
using System;
using Newtonsoft.Json;

namespace Diamond.Core.Clonable.Newtonsoft
{
	/// <summary>
	/// Implements IObjectCloneFactory using Newtonsoft JSON.Net.
	/// </summary>
	public class ObjectCloneFactory : IObjectCloneFactory
	{
		/// <summary>
		/// Creates a deep clone of an object.
		/// </summary>
		/// <param name="instance">The object to clone.</param>
		/// <returns>The new object that s a deep clone of instance.</returns>
		public virtual object CloneInstance(ICloneable instance)
		{
			object returnValue = null;

			//
			// Tell the converter to store the type names.
			//
			JsonSerializerSettings settings = new JsonSerializerSettings()
			{
				ObjectCreationHandling = ObjectCreationHandling.Replace,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
				TypeNameHandling = TypeNameHandling.All,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize
			};

			//
			// Serialize this instance
			//
			string json = JsonConvert.SerializeObject(instance, settings);

			//
			// De-serialize into a new instance
			//
			returnValue = JsonConvert.DeserializeObject(json, settings);

			return returnValue;
		}

		/// <summary>
		/// Creates a deep clone of an object.
		/// </summary>
		/// <typeparam name="T">The type of the instance to clone.</typeparam>
		/// <param name="instance">The object to clone.</param>
		/// <returns>The new object that s a deep clone of instance.</returns>
		public virtual T CloneInstance<T>(T instance)
			where T : ICloneable
		{
			T returnValue = default;

			//
			// Tell the converter to store the type names.
			//
			JsonSerializerSettings settings = new JsonSerializerSettings()
			{
				ObjectCreationHandling = ObjectCreationHandling.Replace,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
				TypeNameHandling = TypeNameHandling.All,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize
			};

			//
			// Serialize this instance
			//
			string json = JsonConvert.SerializeObject(instance, settings);

			//
			// De-serialize into a new instance
			//
			returnValue = JsonConvert.DeserializeObject<T>(json, settings);

			return returnValue;
		}
	}
}

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

namespace Diamond.Core.Repository
{
	/// <summary>
	/// Base entity class allowing generic classes for any
	/// interface defined as an "Entity".
	/// </summary>
	public interface IEntity : ICloneable
	{
	}

	/// <summary>
	/// Base entity class with a "ID" defined as type T. Each
	/// entity (or model) will defined it's own ID type based
	/// on the Mail.dat specification.
	/// </summary>
	/// <typeparam name="T">The type of ID for this entity.</typeparam>
	public interface IEntity<T> : IEntity
	{
		/// <summary>
		/// Get/sets or unique ID for this item.
		/// </summary>
		T Id { get; set; }
	}
}

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

namespace Diamond.Core.Clonable
{
	/// <summary>
	/// Supports cloning, which creates a new instance of a class with the same value
	/// as an existing instance.
	/// </summary>
	public abstract class CloneableObject : ICloneable
	{
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public virtual object Clone()
		{
			return this.OnClone();
		}

		/// <summary>
		/// A class that inherits from Clonable can implement this
		/// member to override the default cloning mechanism.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		protected virtual object OnClone()
		{
			object returnValue = null;

			lock (this)
			{
				IObjectCloneFactory factory = ClonableFactory.GetFactory();
				returnValue = factory.CloneInstance(this);
			}

			return returnValue;
		}
	}
}

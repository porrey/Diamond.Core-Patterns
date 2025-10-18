//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Represents an exception that is thrown when an attempt is made to set a value on a read-only property of a
	/// specified type during dependency injection.
	/// </summary>
	/// <remarks>This exception is typically used in scenarios where dependency injection attempts to assign a value
	/// to a property that is marked as read-only, indicating that the operation is not allowed.</remarks>
	public class PropertyIsReadOnlyException : DependencyInjectionException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyIsReadOnlyException"/> class with the specified type,
		/// property name, and attempted value.
		/// </summary>
		/// <remarks>This exception is thrown when an attempt is made to set a value on a property that is marked as
		/// read-only.</remarks>
		/// <param name="implementationType">The type that contains the read-only property.</param>
		/// <param name="propertyName">The name of the read-only property that was attempted to be set.</param>
		/// <param name="value">The value that was attempted to be assigned to the read-only property.</param>
		public PropertyIsReadOnlyException(Type implementationType, string propertyName, object value)
			: base($"The value '{value}' could not be set on the read-only property '{propertyName}' for type '{implementationType.FullName}'.")
		{
		}

		/// <summary>
		/// Represents an exception that is thrown when an attempt is made to set a value for a read-only property.
		/// </summary>
		/// <param name="implementationType">The type that contains the read-only property.</param>
		/// <param name="propertyName">The name of the read-only property that was attempted to be set.</param>
		public PropertyIsReadOnlyException(Type implementationType, string propertyName)
			: base($"A value for the read-only property '{propertyName}' for type '{implementationType.FullName}' could not be set.")
		{
		}
	}
}

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
namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Represents an exception that is thrown when a specified property is not found on a given type.
	/// </summary>
	/// <remarks>This exception is typically used in dependency injection scenarios where a required property on a
	/// type cannot be resolved. The exception message includes the name of the missing property and the type on which it
	/// was expected to exist.</remarks>
	public class PropertyNotFoundException : DependencyInjectionException
	{
		/// <summary>
		/// Represents an exception that is thrown when a specified property is not found on a given type.
		/// </summary>
		/// <remarks>This exception is typically used to indicate a mismatch between expected and actual properties on
		/// a type, such as during reflection-based operations.</remarks>
		/// <param name="implementationType">The type on which the property was expected to be found.</param>
		/// <param name="propertyName">The name of the property that could not be found.</param>
		public PropertyNotFoundException(Type implementationType, string propertyName)
			: base($"A property named '{propertyName}' was not found on type '{implementationType.FullName}'.")
		{
		}
	}
}

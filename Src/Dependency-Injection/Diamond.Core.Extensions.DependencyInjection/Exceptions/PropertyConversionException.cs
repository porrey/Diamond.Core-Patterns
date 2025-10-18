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
	/// Represents an exception that is thrown when a property value cannot be converted to the required type during
	/// dependency injection.
	/// </summary>
	/// <remarks>This exception is typically thrown when a property on a dependency injection target cannot be
	/// assigned due to a type conversion failure. The exception message includes details about the property, the value
	/// being assigned, and the target type.</remarks>
	public class PropertyConversionException : DependencyInjectionException
	{
		/// <summary>
		/// Represents an exception that is thrown when a property value conversion fails during assignment.
		/// </summary>
		/// <remarks>This exception is typically used to indicate that a value could not be converted to the expected
		/// type for a property, often during object initialization or deserialization.</remarks>
		/// <param name="implementationType">The type of the object where the property resides. This cannot be <see langword="null"/>.</param>
		/// <param name="propertyName">The name of the property for which the conversion failed. This cannot be <see langword="null"/> or empty.</param>
		/// <param name="value">The value that failed to convert to the target property's type.</param>
		/// <param name="innerException">The exception that caused the conversion failure. This cannot be <see langword="null"/>.</param>
		public PropertyConversionException(Type implementationType, string propertyName, object value, Exception innerException)
			: base($"Conversion failed while setting the value '{value}' on property '{propertyName}' for type '{implementationType.FullName}'.", innerException)
		{
		}
	}
}

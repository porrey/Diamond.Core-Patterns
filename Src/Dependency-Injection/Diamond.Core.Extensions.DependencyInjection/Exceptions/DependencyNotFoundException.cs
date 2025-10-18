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
	/// Represents an exception that is thrown when a required dependency for a property in a dependency injection context
	/// cannot be resolved.
	/// </summary>
	/// <remarks>This exception is typically thrown when a dependency injection container is unable to locate a
	/// required dependency for a specific property on a given implementation type. Ensure that all required dependencies
	/// are registered in the container before resolving the type.</remarks>
	public class DependencyNotFoundException : DependencyInjectionException
	{
		/// <summary>
		/// Represents an exception that is thrown when a required dependency for a property on a specific type cannot be
		/// found.
		/// </summary>
		/// <remarks>This exception is typically used to indicate a failure in resolving a dependency during runtime,
		/// such as in dependency injection scenarios.</remarks>
		/// <param name="implementationType">The type that contains the property requiring the dependency.</param>
		/// <param name="propertyName">The name of the property for which the dependency is required.</param>
		/// <param name="dependencyType">The type of the missing dependency.</param>
		public DependencyNotFoundException(Type implementationType, string propertyName, Type dependencyType)
			: base($"The dependency '{dependencyType.FullName}' for property '{propertyName}' on type '{implementationType.FullName}' was not found.")
		{
		}
	}
}

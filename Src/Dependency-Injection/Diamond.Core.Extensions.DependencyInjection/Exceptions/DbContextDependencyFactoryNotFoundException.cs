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
	/// Represents an exception that is thrown when a required DbContext dependency factory cannot be found.
	/// </summary>
	/// <remarks>This exception is typically thrown when the specified factory type is not registered in the
	/// dependency injection container. Ensure that the factory type is correctly registered before attempting to resolve
	/// it.</remarks>
	public class DbContextDependencyFactoryNotFoundException : DependencyInjectionException
	{
		/// <summary>
		/// Represents an exception that is thrown when a specified DbContext dependency factory cannot be found.
		/// </summary>
		/// <param name="factoryType">The name of the factory type that could not be found.</param>
		public DbContextDependencyFactoryNotFoundException(string factoryType)
			: base($"The DbContextDependencyFactory '{factoryType}' was not found.")
		{
		}
	}
}

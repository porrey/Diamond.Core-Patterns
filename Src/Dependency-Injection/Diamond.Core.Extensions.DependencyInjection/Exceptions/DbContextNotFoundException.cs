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
	/// Represents an exception that is thrown when a specified DbContext cannot be found in the dependency injection
	/// container.
	/// </summary>
	/// <remarks>This exception is typically thrown when attempting to resolve a DbContext that has not been
	/// registered in the dependency injection container. Ensure that the DbContext is properly registered in the service
	/// collection before attempting to resolve it.</remarks>
	public class DbContextNotFoundException : DependencyInjectionException
	{
		/// <summary>
		/// Represents an exception that is thrown when a specified DbContext cannot be found.
		/// </summary>
		/// <param name="context">The name of the DbContext that could not be found.</param>
		public DbContextNotFoundException(string context)
			: base($"The DbContext '{context}' was not found.")
		{
		}
	}
}

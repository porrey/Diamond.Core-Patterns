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
	/// Represents an exception that is thrown when a duplicate type alias is encountered in a dependency injection
	/// configuration.
	/// </summary>
	/// <remarks>This exception is typically thrown when attempting to register a type alias with a key that has
	/// already been used.</remarks>
	public class DuplicateAliasException : DependencyInjectionException
	{
		/// <summary>
		/// Represents an exception that is thrown when a duplicate type alias definition is encountered.
		/// </summary>
		/// <param name="key">The key of the duplicate type alias that caused the exception.</param>
		public DuplicateAliasException(string key)
			: base($"A duplicate definition for type alias with the key '{key}' has been encountered.")
		{
		}
	}
}

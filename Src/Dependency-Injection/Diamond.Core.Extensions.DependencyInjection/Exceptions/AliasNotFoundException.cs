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
	/// Represents an exception that is thrown when a type alias with the specified key cannot be found.
	/// </summary>
	public class AliasNotFoundException : DependencyInjectionException
	{
		/// <summary>
		/// Represents an exception that is thrown when a type alias with the specified key is not found.
		/// </summary>
		/// <param name="key">The key of the type alias that could not be found. This value cannot be <see langword="null"/> or empty.</param>
		public AliasNotFoundException(string key)
			: base($"A definition for type alias with the key '{key}' was not found.")
		{
		}
	}
}

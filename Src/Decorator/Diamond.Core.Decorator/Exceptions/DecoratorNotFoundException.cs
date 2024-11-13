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
using Diamond.Core.Abstractions;

namespace Diamond.Core.Decorator
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TItem"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public class DecoratorNotFoundException<TItem, TResult> : DiamondCoreException
	{
		/// <summary>
		/// 
		/// </summary>
		public DecoratorNotFoundException()
			: base($"A decorator of type 'IDecorator<{typeof(TItem).Name}, {typeof(TResult).Name}>' has not been configured.")
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public DecoratorNotFoundException(string name)
			: base($"A decorator of type 'IDecorator<{typeof(TItem).Name}, {typeof(TResult).Name}>' named '{name}' has not been configured.")
		{
		}
	}
}

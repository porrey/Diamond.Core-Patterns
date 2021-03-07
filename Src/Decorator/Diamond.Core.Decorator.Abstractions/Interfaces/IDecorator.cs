//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using System.Threading.Tasks;

namespace Diamond.Core.Decorator
{
	/// <summary>
	/// Defines a generic decorator.
	/// </summary>
	public interface IDecorator
	{
		/// <summary>
		/// 
		/// </summary>
		string Name { get; }
	}

	/// <summary>
	/// Defines a decorator that can has wraps TItem and
	/// returns TResult.
	/// </summary>
	/// <typeparam name="TDecoratedItem">The instance type being decorated.</typeparam>
	/// <typeparam name="TResult">The result of the decorator TakeActionAsync method.</typeparam>
	public interface IDecorator<TDecoratedItem, TResult> : IDecorator
	{
		/// <summary>
		/// Executes the decorator action.
		/// </summary>
		/// <param name="item">The instance of the item being decorated.</param>
		/// <returns>The defined result of the action.</returns>
		Task<TResult> TakeActionAsync(TDecoratedItem item);
	}
}

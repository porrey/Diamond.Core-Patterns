// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Defines a factory to create/retrieve decorator instances.
	/// </summary>
	public interface IDecoratorFactory
	{
		/// <summary>
		/// Gets the specific decorator by type.
		/// </summary>
		/// <typeparam name="TItem">The type of object being decorated.</typeparam>
		/// <typeparam name="TResult">The type of the result returned by the decorator action.</typeparam>
		/// <returns>The result of the decorator action.</returns>
		Task<IDecorator<TItem, TResult>> GetAsync<TItem, TResult>();

		/// <summary>
		/// Gets the specific decorator by type and name.
		/// </summary>
		/// <typeparam name="TItem">The type of object being decorated.</typeparam>
		/// <typeparam name="TResult">The type of the result returned by the decorator action.</typeparam>
		/// <param name="name">The unique name of the decorator.</param>
		/// <returns>The result of the decorator action.</returns>
		Task<IDecorator<TItem, TResult>> GetAsync<TItem, TResult>(string name);
	}
}

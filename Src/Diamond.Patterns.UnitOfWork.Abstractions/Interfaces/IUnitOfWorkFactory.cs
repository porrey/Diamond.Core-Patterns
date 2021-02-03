// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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

namespace Diamond.Patterns.UnitOfWork
{
	/// <summary>
	/// Defines a factory to create/retrieve a specification.
	/// </summary>
	public interface IUnitOfWorkFactory
	{
		/// <summary>
		/// Gets a unit of work that takes in TSourceItem and returns TResult with the given key.
		/// </summary>
		/// <typeparam name="TResult">The type of the result returned by the unit of work execution.</typeparam>
		/// <typeparam name="TSourceItem">The type of the input or parameter passed to the execution
		/// of the unit of work.</typeparam>
		/// <param name="key">A unique key to distinguish this unit of work from other similar definitions.</param>
		/// <returns>The result of the execution as type TResult.</returns>
		Task<IUnitOfWork<TResult, TSourceItem>> GetAsync<TResult, TSourceItem>(string key);
	}
}

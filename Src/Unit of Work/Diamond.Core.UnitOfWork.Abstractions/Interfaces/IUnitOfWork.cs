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

namespace Diamond.Core.UnitOfWork
{
	/// <summary>
	/// Defines a generic unit of work.
	/// </summary>
	public interface IUnitOfWork
	{
		/// <summary>
		/// A unique name to distinguish similar unit of work instances.
		/// </summary>
		string Name { get; }
	}

	/// <summary>
	/// A unit of work that takes TSourceItem and returns type TResult.
	/// </summary>
	/// <typeparam name="TResult">The type of the result returned by the unit of work.</typeparam>
	/// <typeparam name="TSourceItem">The type of the source item for the unit of work.</typeparam>
	public interface IUnitOfWork<TResult, TSourceItem> : IUnitOfWork
	{
		/// <summary>
		/// Executes the unit of work.
		/// </summary>
		/// <param name="item">The source item used in the transaction.</param>
		/// <returns>The result of the action as object instance of type TResult.</returns>
		Task<TResult> CommitAsync(TSourceItem item);
	}
}

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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Diamond.Core.Repository
{
	/// <summary>
	/// Defines a repository that supports read-only queries against a data store. These queries return
	/// an IEnumberable of TInterface.
	/// </summary>
	/// <typeparam name="TInterface"></typeparam>
	public interface IReadOnlyRepository<TInterface> : IRepository<TInterface> where TInterface : IEntity
	{
		/// <summary>
		/// Returns all items in the data store.
		/// </summary>
		/// <returns>Returns an IEnumberable of TInterface</returns>
		Task<IEnumerable<TInterface>> GetAllAsync();

		/// <summary>
		/// Asynchronously retrieves all entities from the repository.
		/// </summary>
		/// <param name="context">The repository context used to access the data source. Cannot be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of entities
		/// of type <typeparamref name="TInterface"/>.</returns>
		Task<IEnumerable<TInterface>> GetAllAsync(IRepositoryContext context);

		/// <summary>
		/// Returns a filtered list of items from the data store.
		/// </summary>
		/// <param name="predicate">Defines the query to be applied before returning the results.</param>
		/// <returns>Returns an IEnumberable of TInterface</returns>
		Task<IEnumerable<TInterface>> GetAsync(Expression<Func<TInterface, bool>> predicate);

		/// <summary>
		/// Asynchronously retrieves a collection of entities that match the specified predicate.
		/// </summary>
		/// <param name="context">The repository context used to access the data source. Cannot be null.</param>
		/// <param name="predicate">An expression that defines the conditions of the entities to retrieve. Cannot be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of entities
		/// that satisfy the specified predicate.</returns>
		Task<IEnumerable<TInterface>> GetAsync(IRepositoryContext context, Expression<Func<TInterface, bool>> predicate);
	}
}

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

namespace Diamond.Core.Repository
{
	/// <summary>
	/// Defines a repository that can be used to modify items in a data store.
	/// </summary>
	/// <typeparam name="TInterface"></typeparam>
	public interface IWritableRepository<TInterface> : IQueryableRepository<TInterface>
		where TInterface : IEntity
	{
		/// <summary>
		/// Gets the model factory used to create new models.
		/// </summary>
		IEntityFactory<TInterface> ModelFactory { get; }

		/// <summary>
		/// Adds a new entity to the data store.
		/// </summary>
		/// <param name="entity">An instance of a new entity.</param>
		/// <returns>Returns true along with an updated entity (if the data store changed
		/// or added data); false otherwise.</returns>
		Task<(bool, TInterface)> AddAsync(TInterface entity);

		/// <summary>
		/// Deletes the specified entity from the data store.
		/// </summary>
		/// <param name="entity">An instance of an existing entity.</param>
		/// <returns></returns>
		Task<bool> DeleteAsync(TInterface entity);

		/// <summary>
		/// Updates an existing entity in the data store.
		/// </summary>
		/// <param name="entity">An instance of an existing entity.</param>
		/// <returns></returns>
		Task<bool> UpdateAsync(TInterface entity);

		/// <summary>
		/// Adds a new entity to the data store using the specified repository
		/// context.
		/// </summary>
		/// <param name="repositoryContext">An existing repository context to use for the transaction.</param>
		/// <param name="entity">An instance of a new entity.</param>
		/// <returns>Returns the updated entity (if the data store changed
		/// or added data)</returns>
		Task<TInterface> AddAsync(IRepositoryContext repositoryContext, TInterface entity);

		/// <summary>
		/// Deletes the specified entity from the data store using the
		/// specified repository context.
		/// </summary>
		/// <param name="repositoryContext"></param>
		/// <param name="entity">An instance of an existing entity.</param>
		/// <returns>Returns true if successful; false otherwise.</returns>
		Task<bool> DeleteAsync(IRepositoryContext repositoryContext, TInterface entity);

		/// <summary>
		/// Updates an existing entity in the data store using the specified
		/// repository context.
		/// </summary>
		/// <param name="repositoryContext"></param>
		/// <param name="entity">An instance of an existing entity.</param>
		/// <returns>Returns true if successful; false otherwise.</returns>
		Task<bool> UpdateAsync(IRepositoryContext repositoryContext, TInterface entity);
	}
}

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

namespace Diamond.Core.Repository {
	/// <summary>
	/// Defines a factory to create/retrieve model repositories.
	/// </summary>
	public interface IRepositoryFactory {
		/// <summary>
		/// Gets a generic <see cref="IRepository"/> for the specified model type.
		/// </summary>
		/// <typeparam name="TInterface">The model type.</typeparam>
		/// <returns>An instance of the repository for the model type specified.</returns>
		Task<IRepository<TInterface>> GetAsync<TInterface>() where TInterface : IEntity;

		/// <summary>
		/// Gets a read-only IReadOnlyRepository for the specified model type.
		/// </summary>
		/// <typeparam name="TInterface">The model type.</typeparam>
		/// <returns></returns>
		Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>() where TInterface : IEntity;

		/// <summary>
		/// Gets a read-only IQueryableRepository for the specified model type.
		/// </summary>
		/// <typeparam name="TInterface">The model type.</typeparam>
		/// <returns>An instance of the repository for the model type specified.</returns>
		Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>() where TInterface : IEntity;

		/// <summary>
		/// Gets a read-only IWritableRepository for the specified model type.
		/// </summary>
		/// <typeparam name="TInterface">The model type.</typeparam>
		/// <returns>An instance of the repository for the model type specified.</returns>
		Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>() where TInterface : IEntity;

		/// <summary>
		/// Gets a generic <see cref="IRepository"/> for the specified model type and name.
		/// </summary>
		/// <typeparam name="TInterface">The model type.</typeparam>
		/// <param name="name"></param>
		/// <returns>An instance of the repository for the model type specified.</returns>
		Task<IRepository<TInterface>> GetAsync<TInterface>(string name = null) where TInterface : IEntity;

		/// <summary>
		/// Gets a read-only IReadOnlyRepository for the specified model type and name.
		/// </summary>
		/// <typeparam name="TInterface">The model type.</typeparam>
		/// <param name="name"></param>
		/// <returns>An instance of the repository for the model type specified.</returns>
		Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>(string name = null) where TInterface : IEntity;

		/// <summary>
		/// Gets a read-only IQueryableRepository for the specified model type and name.
		/// </summary>
		/// <typeparam name="TInterface">The model type.</typeparam>
		/// <param name="name"></param>
		/// <returns>An instance of the repository for the model type specified.</returns>
		Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>(string name = null) where TInterface : IEntity;

		/// <summary>
		/// Gets a read-only IWritableRepository for the specified model type and name.
		/// </summary>
		/// <typeparam name="TInterface">The model type.</typeparam>
		/// <param name="name"></param>
		/// <returns>An instance of the repository for the model type specified.</returns>
		Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>(string name = null) where TInterface : IEntity;
	}
}

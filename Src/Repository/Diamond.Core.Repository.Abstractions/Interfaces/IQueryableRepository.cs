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
using System.Linq;
using System.Threading.Tasks;

namespace Diamond.Core.Repository
{
	/// <summary>
	/// Defines a repository that supports a queryable interface. The connection remains open until specifically
	/// closed by the caller.
	/// </summary>
	/// <typeparam name="TInterface"></typeparam>
	public interface IQueryableRepository<TInterface> : IRepository<TInterface> where TInterface : IEntity
	{
		/// <summary>
		/// Gets an active context that can be used for subsequent queries. This context
		/// can be shared among repositories for the same underlying data store (database).
		/// </summary>
		/// <returns></returns>
		Task<IRepositoryContext> GetContextAsync();

		/// <summary>
		/// Gets an active context that can be used for subsequent queries. This context
		/// can be shared among repositories for the same underlying data store (database).
		/// </summary>
		/// <returns></returns>
		IRepositoryContext GetContext();

		/// <summary>
		/// Gets a <see cref="IQueryable"/> of type TInterface using the current context.
		/// </summary>
		/// <returns>Returns an <see cref="IQueryable"/> of type TInterface.</returns>
		Task<IQueryable<TInterface>> GetQueryableAsync();

		/// <summary>
		/// Gets a <see cref="IQueryable"/> of type TInterface using the current context.
		/// </summary>
		/// <returns>Returns an <see cref="IQueryable"/> of type TInterface.</returns>
		IQueryable<TInterface> GetQueryable();

		/// <summary>
		/// Gets a <see cref="IQueryable"/> of type TInterface using the specified context.
		/// </summary>
		/// <param name="context">A context retrieved from a all to GetContextAsync().</param>
		/// <returns>Returns an <see cref="IQueryable"/> of type TInterface.</returns>
		Task<IQueryable<TInterface>> GetQueryableAsync(IRepositoryContext context);

		/// <summary>
		/// Gets a <see cref="IQueryable"/> of type TInterface using the specified context.
		/// </summary>
		/// <param name="context">A context retrieved from a all to GetContextAsync().</param>
		/// <returns>Returns an <see cref="IQueryable"/> of type TInterface.</returns>
		IQueryable<TInterface> GetQueryable(IRepositoryContext context);
	}
}

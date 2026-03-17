//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Defines a factory for creating search handlers for a specified entity type.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity for which the search handler is created.</typeparam>
	public interface ISearchHandlerFactory<TEntity>
	{
		/// <summary>
		/// Asynchronously retrieves a search handler for the specified property of the entity.
		/// </summary>
		/// <param name="propertyName">The name of the property for which to retrieve the search handler. Cannot be null or empty.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains an <see cref="ISearchHandler{TEntity}"/>
		/// for the specified property.</returns>
		Task<ISearchHandler<TEntity>> GetAsync(string propertyName);
	}
}

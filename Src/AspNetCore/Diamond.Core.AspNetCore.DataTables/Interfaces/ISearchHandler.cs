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
using System.Linq.Expressions;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Specifies the type of search to be performed.
	/// </summary>
	/// <remarks>This enumeration is used to indicate whether a search operation should be conducted globally across
	/// all available data or restricted to a specific column.</remarks>
	public enum SearchType
	{
		/// <summary>
		/// Represents a global search operation that can be performed across multiple data sources.
		/// </summary>
		/// <remarks>This class provides functionality to search for data across various sources, aggregating results
		/// into a unified format. It is designed to handle large datasets efficiently and can be customized to include or
		/// exclude specific data sources.</remarks>
		GlobalSearch,
		/// <summary>
		/// Represents a column in a data table or database schema.
		/// </summary>
		/// <remarks>This class is typically used to define the structure of a table by specifying the properties of
		/// each column, such as its name, data type, and constraints. It can be used in conjunction with other classes to
		/// build and manipulate database schemas programmatically.</remarks>
		Column
	}

	/// <summary>
	/// Defines methods for applying search filters and sorting operations on a collection of entities.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity on which the search and sort operations are performed.</typeparam>
	public interface ISearchHandler<TEntity>
	{
		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		string PropertyName { get; }

		/// <summary>
		/// Asynchronously constructs a search filter expression based on the specified search type and search term.
		/// </summary>
		/// <remarks>The returned expression can be used in LINQ queries to filter entities according to the specified
		/// search type and term. Ensure that the <paramref name="searchTerm"/> is valid and appropriate for the chosen
		/// <paramref name="searchType"/>.</remarks>
		/// <param name="searchType">The type of search to perform, which determines the filtering logic applied.</param>
		/// <param name="searchTerm">The term to search for within the entity data. Cannot be null or empty.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an expression that can be used to
		/// filter entities of type <typeparamref name="TEntity"/> based on the specified search criteria.</returns>
		Task<Expression<Func<TEntity, bool>>> ApplySearchFilterAsync(SearchType searchType, string searchTerm);

		/// <summary>
		/// Sorts the elements of a sequence in ascending or descending order based on a specified column name.
		/// </summary>
		/// <param name="source">The sequence of elements to sort.</param>
		/// <param name="columnName">The name of the column to sort by. This must be a valid property name of the elements in the sequence.</param>
		/// <param name="direction">The direction of the sort. Use "asc" for ascending order or "desc" for descending order.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> whose elements are sorted according to the specified column and
		/// direction.</returns>
		IOrderedQueryable<TEntity> AddOrderBySort(IQueryable<TEntity> source, string columnName, string direction);

		/// <summary>
		/// Adds an additional sorting criterion to the existing ordered query based on the specified column name and
		/// direction.
		/// </summary>
		/// <param name="source">The existing ordered query to which the new sorting criterion will be added.</param>
		/// <param name="columnName">The name of the column by which to sort the query. This must be a valid column name in the query's entity type.</param>
		/// <param name="direction">The direction of the sort, either "asc" for ascending or "desc" for descending. The value is case-insensitive.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> that represents the query with the additional sorting applied.</returns>
		IOrderedQueryable<TEntity> AddThenBySort(IOrderedQueryable<TEntity> source, string columnName, string direction);
	}
}

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
using System.Reflection;
using Diamond.Core.AspNetCore.DataTables;
using LinqKit;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Provides extension methods for working with <see cref="IDataTableRequest"/> and <see cref="IQueryable{TEntity}"/>.
	/// </summary>
	public static class DataTableExtensions
	{
		/// <summary>
		/// Applies ordering to the specified query based on the order criteria provided in the request.
		/// </summary>
		/// <remarks>This method processes the ordering criteria specified in the <paramref name="request"/> and
		/// applies them to the <paramref name="query"/>. If a custom search handler is available for a column, it will be
		/// used to apply the ordering. Otherwise, a default ordering is applied. The method returns a new query with the
		/// specified ordering applied.</remarks>
		/// <typeparam name="TEntity">The type of the entities in the query.</typeparam>
		/// <param name="query">The query to which ordering will be applied.</param>
		/// <param name="request">The data table request containing the ordering criteria.</param>
		/// <param name="searchHandlerFactory">The factory used to obtain search handlers for custom ordering logic.</param>
		/// <returns>An <see cref="IQueryable{TEntity}"/> with the applied ordering.</returns>
		public static IQueryable<TEntity> ApplyOrdering<TEntity>(this IQueryable<TEntity> query, IDataTableRequest request, ISearchHandlerFactory<TEntity> searchHandlerFactory)
		{
			IQueryable<TEntity> returnValue = query;

			//
			// Get the ordered columns.
			//
			IEnumerable<OrderedColumn> orderedColumns = request.OrderedColumns<TEntity>();

			//
			// Apply ordering
			//
			if (orderedColumns.Any())
			{
				//
				// Build an ordered queryable.
				//
				IOrderedQueryable<TEntity> q = null;

				foreach (OrderedColumn orderedColumn in orderedColumns.Where(t => !string.IsNullOrWhiteSpace(t.ColumnName)))
				{
					if (orderedColumn == orderedColumns.First())
					{
						ISearchHandler<TEntity> handler = searchHandlerFactory.GetAsync(orderedColumn.ColumnName).Result;

						if (handler != null)
						{
							q = handler.AddOrderBySort(query, orderedColumn.ColumnName, orderedColumn.Direction);
						}
						else
						{
							q = query.AddOrderBySort(orderedColumn.ColumnName, orderedColumn.Direction);
						}
					}
					else
					{
						ISearchHandler<TEntity> handler = searchHandlerFactory.GetAsync(orderedColumn.ColumnName).Result;

						if (handler != null)
						{
							q = handler.AddThenBySort(q, orderedColumn.ColumnName, orderedColumn.Direction);
						}
						else
						{
							q = q.AddThenBySort(orderedColumn.ColumnName, orderedColumn.Direction);
						}
					}
				}

				//
				// We have an ordered queryable, set the return value.
				//
				returnValue = q;
			}

			return returnValue;
		}

		/// <summary>
		/// Applies paging to the specified query based on the provided data table request.
		/// </summary>
		/// <remarks>The method uses the <see cref="IDataTableRequest.Start"/> and <see
		/// cref="IDataTableRequest.Length"/> properties to determine the starting point and the number of elements to return.
		/// If <see cref="IDataTableRequest.Length"/> is less than or equal to zero, all elements from the starting point are
		/// returned.</remarks>
		/// <typeparam name="TEntity">The type of the elements in the query.</typeparam>
		/// <param name="query">The query to which paging will be applied.</param>
		/// <param name="request">The data table request containing paging parameters. If <paramref name="request"/> is null, the query is returned
		/// without paging.</param>
		/// <returns>An <see cref="IEnumerable{TEntity}"/> that contains the elements of the query after applying the specified paging
		/// parameters. If <paramref name="request"/> is null, the original query is returned.</returns>
		public static IEnumerable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> query, IDataTableRequest request)
		{
			IEnumerable<TEntity> returnValue = [];

			if (request != null)
			{
				if (request.Start >= 0)
				{
					if (request.Length > 0)
					{
						returnValue = query.Skip(request.Start).Take(request.Length);
					}
					else
					{
						returnValue = query.Skip(request.Start);
					}
				}
			}
			else
			{
				returnValue = query;
			}

			return returnValue;
		}

		/// <summary>
		/// Constructs a search expression for filtering entities based on the search criteria specified in the request.
		/// </summary>
		/// <remarks>This method generates a dynamic search expression by iterating over the properties of the
		/// specified view model type. For each property, it uses a search handler to create a filter expression, which is
		/// combined into a single expression using logical OR operations. If no search criteria are provided, the method
		/// returns an expression that evaluates to <see langword="true"/>.</remarks>
		/// <typeparam name="TEntity">The type of the entity to be filtered.</typeparam>
		/// <typeparam name="TViewModel">The type of the view model containing properties to be searched.</typeparam>
		/// <param name="request">The data table request containing search parameters. Must not be null.</param>
		/// <param name="searchHandlerFactory">The factory used to obtain search handlers for each property. Must not be null.</param>
		/// <returns>An expression that evaluates to <see langword="true"/> for entities matching the search criteria, or <see
		/// langword="false"/> if no criteria are specified.</returns>
		public static Expression<Func<TEntity, bool>> ApplySearch<TEntity, TViewModel>(this IDataTableRequest request, ISearchHandlerFactory<TEntity> searchHandlerFactory)
		{
			Expression<Func<TEntity, bool>> returnValue = null;

			if (request != null && request.Search != null && !string.IsNullOrWhiteSpace(request.Search.Value))
			{
				returnValue = PredicateBuilder.New<TEntity>(false);
				PropertyInfo[] properties = typeof(TViewModel).GetProperties();

				foreach (PropertyInfo property in properties)
				{
					ISearchHandler<TEntity> handler = searchHandlerFactory.GetAsync(property.Name).Result;

					if (handler != null)
					{
						Expression<Func<TEntity, bool>> filterExpression = handler.ApplySearchFilterAsync(SearchType.GlobalSearch, request.Search.Value).Result;

						if (filterExpression != null)
						{
							returnValue = returnValue.Or(filterExpression);
						}
					}
				}
			}
			else
			{
				returnValue = PredicateBuilder.New<TEntity>(true);
			}

			return returnValue;
		}

		/// <summary>
		/// Constructs a filter expression for the specified entity type based on the search criteria provided in the data
		/// table request.
		/// </summary>
		/// <remarks>This method constructs a filter expression by examining the search criteria specified in the
		/// <paramref name="request"/>. It uses the <paramref name="searchHandlerFactory"/> to obtain handlers that apply the
		/// search filters to the corresponding entity properties. The resulting expression can be used to filter a collection
		/// of entities based on the specified search criteria.</remarks>
		/// <typeparam name="TEntity">The type of the entity to which the filter will be applied.</typeparam>
		/// <typeparam name="TViewModel">The type of the view model that contains properties corresponding to the entity's columns.</typeparam>
		/// <param name="request">The data table request containing the search criteria for filtering.</param>
		/// <param name="searchHandlerFactory">The factory used to obtain search handlers for applying search filters to entity properties.</param>
		/// <returns>An expression that represents the combined filter criteria for the entity type. Returns a default expression that
		/// evaluates to <see langword="true"/> if no search criteria are provided.</returns>
		public static Expression<Func<TEntity, bool>> ApplyFilter<TEntity, TViewModel>(this IDataTableRequest request, ISearchHandlerFactory<TEntity> searchHandlerFactory)
		{
			Expression<Func<TEntity, bool>> returnValue = PredicateBuilder.New<TEntity>(true);

			if (request != null)
			{
				var filteredColumns = (from tbl1 in request.Columns
									   where !string.IsNullOrWhiteSpace(tbl1.Search.Value)
									   select new
									   {
										   ColumnName = tbl1.Data,
										   tbl1.Search
									   }).ToArray();

				if (filteredColumns.Any())
				{
					var items = (from tbl1 in typeof(TViewModel).GetProperties()
								 join tbl2 in filteredColumns on tbl1.Name.ToLower() equals tbl2.ColumnName.ToLower()
								 select new
								 {
									 Property = tbl1,
									 Search = tbl2.Search
								 }).ToArray();

					foreach (var item in items)
					{
						ISearchHandler<TEntity> handler = searchHandlerFactory.GetAsync(item.Property.Name).Result;

						if (handler != null)
						{
							Expression<Func<TEntity, bool>> filterExpression = handler.ApplySearchFilterAsync(SearchType.Column, item.Search.Value).Result;

							if (filterExpression != null)
							{
								returnValue = returnValue.And(filterExpression);
							}
						}
					}
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Returns the zero-based index of the first element in the sequence that satisfies the specified predicate.
		/// </summary>
		/// <typeparam name="TEntity">The type of the elements in the sequence.</typeparam>
		/// <param name="source">The sequence to search.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The zero-based index of the first element that satisfies the predicate; otherwise, the number of elements in the
		/// sequence if no such element is found.</returns>
		public static int IndexOf<TEntity>(this IEnumerable<TEntity> source, Func<TEntity, bool> predicate)
		{
			int index = 0;

			foreach (TEntity item in source)
			{
				if (predicate.Invoke(item))
				{
					break;
				}
				else
				{
					index++;
				}
			}

			return index;
		}

		/// <summary>
		/// Sorts the elements of a sequence in ascending or descending order based on a specified column name.
		/// </summary>
		/// <typeparam name="TEntity">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <param name="source">The sequence of elements to sort.</param>
		/// <param name="columnName">The name of the column to sort by. This must be a valid property name of <typeparamref name="TEntity"/>.</param>
		/// <param name="direction">The direction of the sort. Use "asc" for ascending order or any other value for descending order.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> whose elements are sorted according to the specified column and
		/// direction.</returns>
		public static IOrderedQueryable<TEntity> AddOrderBySort<TEntity>(this IQueryable<TEntity> source, string columnName, string direction)
		{
			IOrderedQueryable<TEntity> returnValue = null;

			if (direction.ToLower() == "asc")
			{
				returnValue = source.OrderBy(columnName);
			}
			else
			{
				returnValue = source.OrderByDescending(columnName);
			}

			return returnValue;
		}

		/// <summary>
		/// Adds a secondary sorting criterion to the existing ordered query based on the specified column name and direction.
		/// </summary>
		/// <typeparam name="TEntity">The type of the elements in the source query.</typeparam>
		/// <param name="source">The existing ordered query to which the secondary sort will be applied.</param>
		/// <param name="columnName">The name of the column to sort by. This must be a valid property name of <typeparamref name="TEntity"/>.</param>
		/// <param name="direction">The direction of the sort. Use "asc" for ascending order or "desc" for descending order.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> that represents the query with the additional sorting applied.</returns>
		public static IOrderedQueryable<TEntity> AddThenBySort<TEntity>(this IOrderedQueryable<TEntity> source, string columnName, string direction)
		{
			IOrderedQueryable<TEntity> returnValue = null;

			if (direction.ToLower() == "asc")
			{
				returnValue = source.ThenBy(columnName);
			}
			else
			{
				returnValue = source.ThenByDescending(columnName);
			}

			return returnValue;
		}

		/// <summary>
		/// Creates a lambda expression that accesses a specified property of a given entity type.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity containing the property.</typeparam>
		/// <param name="propertyName">The name of the property to access. This parameter is case-insensitive.</param>
		/// <returns>An <see cref="Expression{TDelegate}"/> representing a lambda expression that accesses the specified property of
		/// the entity type <typeparamref name="TEntity"/> and returns its value as an <see cref="object"/>.</returns>
		public static Expression<Func<TEntity, object>> GetExpression<TEntity>(string propertyName)
		{
			PropertyInfo property = typeof(TEntity).GetProperties().Where(t => t.Name.ToLower() == propertyName.ToLower()).SingleOrDefault();
			ParameterExpression param = Expression.Parameter(typeof(TEntity), "t");
			Expression conversion = Expression.Convert(Expression.Property(param, property), typeof(object));

			return Expression.Lambda<Func<TEntity, object>>(conversion, param);
		}

		/// <summary>
		/// Sorts the elements of a sequence in ascending order according to a specified property name.
		/// </summary>
		/// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
		/// <param name="source">The sequence of elements to order.</param>
		/// <param name="propertyName">The name of the property to sort the elements by. This property must exist on <typeparamref name="TEntity"/>.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> whose elements are sorted according to the specified property.</returns>
		public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName)
		{
			return source.OrderBy(GetExpression<TEntity>(propertyName));
		}

		/// <summary>
		/// Sorts the elements of a sequence in descending order according to a specified property name.
		/// </summary>
		/// <typeparam name="TEntity">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <param name="source">The sequence of elements to order.</param>
		/// <param name="propertyName">The name of the property to sort by. This property must exist on <typeparamref name="TEntity"/>.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> whose elements are sorted in descending order according to the
		/// specified property.</returns>
		public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string propertyName)
		{
			return source.OrderByDescending(GetExpression<TEntity>(propertyName));
		}

		/// <summary>
		/// Performs a subsequent ordering of the elements in a sequence in ascending order according to a specified property
		/// name.
		/// </summary>
		/// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
		/// <param name="source">An <see cref="IOrderedQueryable{TEntity}"/> that contains elements to be sorted.</param>
		/// <param name="propertyName">The name of the property to sort by. This must be a valid property name of <typeparamref name="TEntity"/>.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> whose elements are sorted according to the specified property.</returns>
		public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string propertyName)
		{
			return source.ThenBy(GetExpression<TEntity>(propertyName));
		}

		/// <summary>
		/// Performs a subsequent ordering of the elements in a sequence in descending order, according to a specified
		/// property name.
		/// </summary>
		/// <typeparam name="TEntity">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <param name="source">An <see cref="IOrderedQueryable{TEntity}"/> that contains elements to sort.</param>
		/// <param name="propertyName">The name of the property to sort by. This must be a valid property name of <typeparamref name="TEntity"/>.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> whose elements are sorted in descending order according to the
		/// specified property.</returns>
		public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source, string propertyName)
		{
			return source.ThenByDescending(GetExpression<TEntity>(propertyName));
		}

		/// <summary>
		/// Retrieves an ordered collection of columns based on the specified order criteria in the request.
		/// </summary>
		/// <remarks>This method processes the columns specified in the <paramref name="request"/> and applies the
		/// ordering criteria defined in the request's order property. Each <see cref="OrderedColumn"/> in the result contains
		/// the column name and the direction of sorting.</remarks>
		/// <typeparam name="TEntity">The type of the entity associated with the data table request.</typeparam>
		/// <param name="request">The data table request containing column and order information. Cannot be null.</param>
		/// <returns>An <see cref="IEnumerable{OrderedColumn}"/> representing the columns ordered according to the request. Returns an
		/// empty collection if no ordering is specified or if the request is null.</returns>
		public static IEnumerable<OrderedColumn> OrderedColumns<TEntity>(this IDataTableRequest request)
		{
			IEnumerable<OrderedColumn> returnValue = [];

			//
			// Apply ordering
			//
			if (request != null && request.Order != null && request.Order.Length > 0)
			{
				returnValue = (from tbl1 in request.Columns
							   join tbl2 in request.Order on request.Columns.IndexOf(t => t.Data == tbl1.Data) equals tbl2.Column
							   where !string.IsNullOrWhiteSpace(tbl1.Data)
							   select new OrderedColumn()
							   {
								   ColumnName = tbl1.Data,
								   Direction = tbl2.Dir
							   }).ToArray();
			}

			return returnValue;
		}
	}
}
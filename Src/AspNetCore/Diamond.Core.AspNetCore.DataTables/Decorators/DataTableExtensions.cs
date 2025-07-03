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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Diamond.Core.AspNetCore.DataTables;
using LinqKit;

namespace Diamond.Core.AspNetCore.DataTables
{
	public static class DataTableExtensions
	{
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

		public static Expression<Func<TEntity, object>> GetExpression<TEntity>(string propertyName)
		{
			PropertyInfo property = typeof(TEntity).GetProperties().Where(t => t.Name.ToLower() == propertyName.ToLower()).SingleOrDefault();
			ParameterExpression param = Expression.Parameter(typeof(TEntity), "t");
			Expression conversion = Expression.Convert(Expression.Property(param, property), typeof(object));

			return Expression.Lambda<Func<TEntity, object>>(conversion, param);
		}

		public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName)
		{
			return source.OrderBy(GetExpression<TEntity>(propertyName));
		}

		public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string propertyName)
		{
			return source.OrderByDescending(GetExpression<TEntity>(propertyName));
		}

		public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string propertyName)
		{
			return source.ThenBy(GetExpression<TEntity>(propertyName));
		}

		public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source, string propertyName)
		{
			return source.ThenByDescending(GetExpression<TEntity>(propertyName));
		}

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
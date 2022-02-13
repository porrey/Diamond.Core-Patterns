//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
	public static class DataTableExtensions
	{
		public static IQueryable<TEntity> ApplyOrdering<TEntity>(this IQueryable<TEntity> query, IDataTableRequest request)
		{
			IQueryable<TEntity> returnValue = query;

			//
			// Apply ordering
			//
			if (request != null && request.Order != null && request.Order.Length > 0)
			{
				var orderedColumns = (from tbl1 in request.Columns
									  join tbl2 in request.Order on request.Columns.IndexOf(t => t.Data == tbl1.Data) equals tbl2.Column
									  select new
									  {
										  ColumnName = tbl1.Data,
										  Direction = tbl2.Dir
									  }).ToArray();

				foreach (var orderedColumn in orderedColumns)
				{
					returnValue = returnValue.AddSort<TEntity>(orderedColumn.ColumnName, orderedColumn.Direction);
				}
			}

			return returnValue;
		}

		public static TEntity[] FinalizeQuery<TEntity>(this IQueryable<TEntity> query, IDataTableRequest request)
		{
			TEntity[] returnValue = Array.Empty<TEntity>();

			if (request != null)
			{
				if (request.Start >= 0)
				{
					if (request.Length > 0)
					{
						returnValue = query.Skip(request.Start).Take(request.Length).ToArray();
					}
					else
					{
						returnValue = query.Skip(request.Start).ToArray();
					}
				}
			}
			else
			{
				returnValue = query.ToArray();
			}

			return returnValue;
		}

		public static int IndexOf<TEntity>(this IEnumerable<TEntity> source, Func<TEntity, bool> predicate)
		{
			int index = 0;

			foreach (var item in source)
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

		public static IOrderedQueryable<TEntity> AddSort<TEntity>(this IQueryable<TEntity> source, string columnName, string direction)
		{
			IOrderedQueryable<TEntity> returnValue = null;

			Type ts = typeof(TEntity);

			var names = (from tbl in ts.GetProperties()
						 where tbl.Name.ToLower() == columnName.ToLower()
						 select tbl).ToArray();

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
						returnValue = returnValue.Or(filterExpression);
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
							returnValue = returnValue.And(filterExpression);
						}
					}
				}
			}

			return returnValue;
		}
	}
}
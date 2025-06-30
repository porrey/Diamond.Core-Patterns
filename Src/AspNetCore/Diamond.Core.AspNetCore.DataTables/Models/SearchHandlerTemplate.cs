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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Provides a base implementation for handling search and sorting operations on entities of type <typeparamref
	/// name="TEntity"/>.
	/// </summary>
	/// <remarks>This abstract class serves as a template for implementing search and sorting functionality. It
	/// provides default behavior for applying search filters, ordering queryable sources, and logging operations. Derived
	/// classes can override the provided virtual methods to customize the behavior for specific use cases.</remarks>
	/// <typeparam name="TEntity">The type of the entity that the search handler operates on.</typeparam>
	public abstract class SearchHandlerTemplate<TEntity> : ISearchHandler<TEntity>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SearchHandlerTemplate{TEntity}"/> class.
		/// </summary>
		/// <remarks>The logger is used to capture diagnostic and operational information during the execution of the
		/// search handler. Ensure that a valid <see cref="ILogger{TCategoryName}"/> implementation is provided to avoid
		/// runtime issues.</remarks>
		/// <param name="logger">The logger instance used to log messages for the <see cref="SearchHandlerTemplate{TEntity}"/>.</param>
		public SearchHandlerTemplate(ILogger<SearchHandlerTemplate<TEntity>> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Gets or sets the logger used for logging operations within the search handler.
		/// </summary>
		/// <remarks>Override this property to provide a custom logger implementation if logging is
		/// required.</remarks>
		protected virtual ILogger<SearchHandlerTemplate<TEntity>> Logger { get; set; } = new NullLogger<SearchHandlerTemplate<TEntity>>();

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		public virtual string PropertyName { get; }

		/// <summary>
		/// Asynchronously applies a search or filter operation based on the specified search type and search term.
		/// </summary>
		/// <remarks>This method determines the appropriate search or filter logic based on the <paramref
		/// name="searchType"/> and delegates the  operation to either the <c>OnSearch</c> or <c>OnFilter</c> method. If an
		/// exception occurs during the operation, it is logged,  and a default expression is returned.</remarks>
		/// <param name="searchType">The type of search to perform. Use <see cref="SearchType.GlobalSearch"/> for a global search or other values for
		/// specific filtering.</param>
		/// <param name="searchTerm">The term to search or filter by. This value is used to generate the search or filter expression.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an expression that evaluates to a
		/// boolean value,  representing the search or filter criteria to be applied to the entity.</returns>
		public virtual Task<Expression<Func<TEntity, bool>>> ApplySearchFilterAsync(SearchType searchType, string searchTerm)
		{
			Expression<Func<TEntity, bool>> returnValue = searchType == SearchType.GlobalSearch ? t => false : t => true;

			try
			{
				this.Logger.LogDebug("{type} search requested for search term '{term}' on property '{name}'.", searchType, searchTerm, this.PropertyName);

				if (searchType == SearchType.GlobalSearch)
				{
					returnValue = this.OnSearch(searchTerm);
				}
				else
				{
					returnValue = this.OnFilter(searchTerm);
				}
			}
			catch (Exception ex)
			{
				this.Logger.LogError(ex, "Exception in SearchHandlerTemplate for '{name}'.", this.PropertyName);
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// Applies an ordering to the specified queryable source based on the given column name and sort direction.
		/// </summary>
		/// <remarks>This method delegates the ordering logic to the <see cref="OnAddOrderBySort"/> method, which can
		/// be overridden to customize behavior.</remarks>
		/// <param name="source">The queryable source to which the ordering will be applied. Cannot be null.</param>
		/// <param name="columnName">The name of the column to sort by. Must correspond to a valid property of <typeparamref name="TEntity"/>.</param>
		/// <param name="direction">The direction of the sort. Use <see langword="asc"/> for ascending order or <see langword="desc"/> for descending
		/// order.</param>
		/// <returns>An <see cref="IOrderedQueryable{T}"/> representing the source with the specified ordering applied.</returns>
		public virtual IOrderedQueryable<TEntity> AddOrderBySort(IQueryable<TEntity> source, string columnName, string direction)
		{
			return this.OnAddOrderBySort(source, columnName, direction);
		}

		/// <summary>
		/// Adds an additional sorting condition to an already ordered queryable source.
		/// </summary>
		/// <param name="source">The ordered queryable source to which the additional sorting condition will be applied.</param>
		/// <param name="columnName">The name of the column to sort by. Must match a valid property of the entity type.</param>
		/// <param name="direction">The direction of the sort. Use <see langword="asc"/> for ascending or <see langword="desc"/> for descending.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> representing the source with the additional sorting condition applied.</returns>
		public virtual IOrderedQueryable<TEntity> AddThenBySort(IOrderedQueryable<TEntity> source, string columnName, string direction)
		{
			return this.OnAddThenBySort(source, columnName, direction);
		}

		/// <summary>
		/// Applies an ordering to the specified queryable source based on the given column name and sort direction.
		/// </summary>
		/// <remarks>This method is virtual and can be overridden to customize the ordering behavior.  Ensure that the
		/// <paramref name="columnName"/> corresponds to a valid property of <typeparamref name="TEntity"/>.</remarks>
		/// <param name="source">The queryable source to which the ordering will be applied.</param>
		/// <param name="columnName">The name of the column to sort by. This parameter is case-insensitive.</param>
		/// <param name="direction">The sort direction, specified as "asc" for ascending or "desc" for descending.  If the value is not "asc",
		/// descending order is applied by default.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> representing the source with the specified ordering applied.</returns>
		protected virtual IOrderedQueryable<TEntity> OnAddOrderBySort(IQueryable<TEntity> source, string columnName, string direction)
		{
			IOrderedQueryable<TEntity> returnValue = null;

			Type ts = typeof(TEntity);

			IEnumerable<PropertyInfo> names = from tbl in ts.GetProperties()
											  where tbl.Name.ToLower() == columnName.ToLower()
											  select tbl;

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
		/// Adds an additional sorting condition to an already ordered queryable source.
		/// </summary>
		/// <remarks>This method is intended to be overridden in derived classes to customize the behavior of adding
		/// additional sorting conditions. The default implementation applies the sorting based on the specified column name
		/// and direction.</remarks>
		/// <param name="source">The <see cref="IOrderedQueryable{TEntity}"/> to which the additional sorting condition will be applied.</param>
		/// <param name="columnName">The name of the column to sort by. This parameter is case-insensitive.</param>
		/// <param name="direction">The direction of the sort. Use <see langword="asc"/> for ascending order or <see langword="desc"/> for descending
		/// order.</param>
		/// <returns>An <see cref="IOrderedQueryable{TEntity}"/> with the additional sorting condition applied.</returns>
		protected virtual IOrderedQueryable<TEntity> OnAddThenBySort(IOrderedQueryable<TEntity> source, string columnName, string direction)
		{
			IOrderedQueryable<TEntity> returnValue = null;

			Type ts = typeof(TEntity);

			IEnumerable<PropertyInfo> names = from tbl in ts.GetProperties()
											  where tbl.Name.ToLower() == columnName.ToLower()
											  select tbl;

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
		/// Constructs a search expression based on the provided search term.
		/// </summary>
		/// <remarks>This method is intended to be overridden in derived classes to provide custom search logic. The
		/// default implementation returns <see langword="null"/>, indicating no filtering.</remarks>
		/// <param name="searchTerm">The term to be used for filtering entities. Can be null or empty, in which case no filtering is applied.</param>
		/// <returns>An expression that represents the search filter to be applied to entities of type <typeparamref name="TEntity"/>.
		/// Returns <see langword="null"/> if no filtering is required.</returns>
		protected virtual Expression<Func<TEntity, bool>> OnSearch(string searchTerm)
		{
			return null;
		}

		/// <summary>
		/// Provides a filter expression to be applied to a query based on the specified search term.
		/// </summary>
		/// <param name="searchTerm">The search term used to generate the filter expression. Can be null or empty.</param>
		/// <returns>An expression that represents the filter to apply to the query.  Returns <see langword="null"/> if no filter is
		/// applied.</returns>
		protected virtual Expression<Func<TEntity, bool>> OnFilter(string searchTerm)
		{
			return null;
		}
	}
}

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
using AutoMapper;
using Diamond.Core.AspNetCore.DoAction;
using Diamond.Core.Repository;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Provides a template for handling data table queries with support for filtering, searching, and paging.
	/// </summary>
	/// <remarks>This abstract class serves as a base for implementing data table query handlers. It provides a
	/// structured approach to executing queries with filtering, searching, and paging capabilities. Derived classes can
	/// override the provided virtual methods to customize the query execution process.</remarks>
	/// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
	/// <typeparam name="TViewModel">The type of the view model used to represent the entity in the result.</typeparam>
	/// <typeparam name="TRequest">The type of the request containing query parameters.</typeparam>
	public abstract class DataTableQueryHandlerTemplate<TEntity, TViewModel, TRequest> : IDataTableQueryHandler<TEntity, TViewModel, TRequest>
		where TEntity : IEntity
		where TRequest : IDataTableRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataTableQueryHandlerTemplate{TEntity, TViewModel, TRequest}"/>
		/// class.
		/// </summary>
		/// <param name="logger">The logger instance used for logging operations within the query handler.</param>
		/// <param name="repositoryFactory">The factory used to create repository instances for data access.</param>
		/// <param name="searchHandlerFactory">The factory used to create search handlers for processing search queries on <typeparamref name="TEntity"/>.</param>
		/// <param name="mapper">The mapper used to map between <typeparamref name="TEntity"/> and <typeparamref name="TViewModel"/>.</param>
		public DataTableQueryHandlerTemplate(ILogger<DataTableQueryHandlerTemplate<TEntity, TViewModel, TRequest>> logger, IRepositoryFactory repositoryFactory, ISearchHandlerFactory<TEntity> searchHandlerFactory, IMapper mapper)
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
			this.SearchHandlerFactory = searchHandlerFactory;
			this.Mapper = mapper;
		}

		/// <summary>
		/// Gets or sets the logger used for logging operations within the query handler.
		/// </summary>
		protected virtual ILogger<DataTableQueryHandlerTemplate<TEntity, TViewModel, TRequest>> Logger { get; set; }

		/// <summary>
		/// Gets or sets the repository factory used to create repository instances for data access.
		/// </summary>
		protected virtual IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// Gets or sets the search handler factory used to create search handlers for processing search queries on <typeparamref name="TEntity"/>.
		/// </summary>
		protected virtual ISearchHandlerFactory<TEntity> SearchHandlerFactory { get; set; }

		/// <summary>
		/// Gets or sets the mapper used to map between <typeparamref name="TEntity"/> and <typeparamref name="TViewModel"/>.
		/// </summary>
		protected virtual IMapper Mapper { get; set; }

		/// <summary>
		/// Executes an asynchronous query based on the specified request and initial expression, returning a result set of
		/// view models.
		/// </summary>
		/// <remarks>This method retrieves a repository for the specified entity type and constructs a query using the
		/// provided initial expression, along with additional filter and search expressions. It executes the query and maps
		/// the resulting entities to view models. The result includes the total number of records, the number of filtered
		/// records, and the data set.</remarks>
		/// <param name="request">The request object containing query parameters and options. Cannot be null.</param>
		/// <param name="initialExpression">An initial filter expression to apply to the query. This expression is combined with additional filters and search
		/// criteria.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains a <see
		/// cref="IControllerActionResult{T}"/> with a <see cref="DataTableResult{TViewModel}"/> that includes the queried
		/// data, draw count, and record counts.</returns>
		public virtual async Task<IControllerActionResult<DataTableResult<TViewModel>>> ExecuteQueryAsync(TRequest request, Expression<Func<TEntity, bool>> initialExpression)
		{
			ControllerActionResult<DataTableResult<TViewModel>> returnValue = new();

			request = this.OnRequestStarted(request);

			//
			// Get the repository.
			//
			this.Logger.LogDebug("Retrieving repository for '{type}'.", typeof(TEntity).Name);
			using IQueryableRepository<TEntity> repository = await this.RepositoryFactory.GetQueryableAsync<TEntity>();

			//
			// Get the filter expression.
			//
			this.Logger.LogDebug("Retrieving column filter expression.");
			Expression<Func<TEntity, bool>> filterExpression = this.OnGetFilterExpression(request);

			//
			// Get the search expression.
			//
			this.Logger.LogDebug("Retrieving global search expression.");
			Expression<Func<TEntity, bool>> searchExpression = this.OnGetSearchExpression(request);

			//
			// Get the final expression.
			//
			this.Logger.LogDebug("Retrieving global search expression.");
			Expression<Func<TEntity, bool>> finalExpression = this.OnBuildExpression(initialExpression, filterExpression, searchExpression);

			//
			// Execute a query
			//
			this.Logger.LogDebug("Executing query.");
			IEnumerable<TEntity> items = this.OnExecuteQuery(request, finalExpression, repository);

			//
			// Get the total record count.
			//
			int totalCount = repository.GetQueryable().Where(initialExpression).Count();

			//
			// Get the total number of filtered records.
			//
			int filteredCount = repository.GetQueryable().Where(finalExpression).Count();

			//
			// Get the count of records being returned (this may be less due to paging).
			//
			int returnCount = items.Count();

			//
			// Check if the data is being filtered.
			//
			bool isFiltered = filteredCount != totalCount;

			//
			// Set the grid properties.
			//
			returnValue.ResultDetails = DoActionResult.Ok();
			returnValue.Result = new()
			{
				Data = [.. this.Mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(items)],
				Draw = request != null ? request.Draw : 1,
				RecordsFiltered = isFiltered ? filteredCount : totalCount,
				RecordsTotal = totalCount
			};

			return this.OnRequestCompleted(request, returnValue);
		}

		/// <summary>
		/// Invoked when a request is initiated, allowing for customization or logging.
		/// </summary>
		/// <remarks>This method can be overridden in a derived class to perform additional actions when a request
		/// starts, such as logging or modifying the request. The default implementation returns the request
		/// unchanged.</remarks>
		/// <param name="request">The request object that is being started. This parameter cannot be null.</param>
		/// <returns>The original or modified request object to be processed.</returns>
		protected virtual TRequest OnRequestStarted(TRequest request)
		{
			return request;
		}

		/// <summary>
		/// Constructs a filter expression based on the specified request.
		/// </summary>
		/// <remarks>This method uses the provided request to generate a filter expression that can be used to query
		/// entities. Override this method to customize the filter expression logic.</remarks>
		/// <param name="request">The request containing filter criteria to apply.</param>
		/// <returns>An expression that represents the filter to be applied to entities of type <typeparamref name="TEntity"/>.</returns>
		protected virtual Expression<Func<TEntity, bool>> OnGetFilterExpression(TRequest request)
		{
			return request.ApplyFilter<TEntity, TViewModel>(this.SearchHandlerFactory);
		}

		/// <summary>
		/// Constructs a search expression based on the specified request.
		/// </summary>
		/// <remarks>This method uses the <see cref="SearchHandlerFactory"/> to apply the search logic defined in the
		/// request. Override this method to customize the search expression generation for specific request types.</remarks>
		/// <param name="request">The request containing search criteria and parameters.</param>
		/// <returns>An expression that represents the search criteria to be applied to entities of type <typeparamref
		/// name="TEntity"/>.</returns>
		protected virtual Expression<Func<TEntity, bool>> OnGetSearchExpression(TRequest request)
		{
			return request.ApplySearch<TEntity, TViewModel>(this.SearchHandlerFactory);
		}

		/// <summary>
		/// Builds a combined expression by logically AND-ing the provided expressions.
		/// </summary>
		/// <param name="initialExpression">The initial expression to start the combination process.</param>
		/// <param name="filterExpression">An expression used to filter the entities.</param>
		/// <param name="searchExpression">An expression used to search within the entities.</param>
		/// <returns>An expression that represents the logical AND combination of the initial, filter, and search expressions.</returns>
		protected virtual Expression<Func<TEntity, bool>> OnBuildExpression(Expression<Func<TEntity, bool>> initialExpression, Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, bool>> searchExpression)
		{
			return initialExpression.And(filterExpression).And(searchExpression);
		}

		/// <summary>
		/// Executes a query on the specified repository using the provided request and filter expression.
		/// </summary>
		/// <remarks>This method applies ordering and paging to the query based on the parameters specified in the
		/// <paramref name="request"/>. If the request contains ordered columns, the query will be ordered accordingly before
		/// applying the filter and paging.</remarks>
		/// <param name="request">The request object containing parameters for ordering and paging the query results.</param>
		/// <param name="expression">An expression used to filter the entities in the repository.</param>
		/// <param name="repository">The repository from which the entities are queried.</param>
		/// <returns>An <see cref="IEnumerable{TEntity}"/> containing the entities that match the specified filter and query
		/// parameters.</returns>
		protected virtual IEnumerable<TEntity> OnExecuteQuery(TRequest request, Expression<Func<TEntity, bool>> expression, IQueryableRepository<TEntity> repository)
		{
			if (request.OrderedColumns<TEntity>().Any())
			{
				return repository.GetQueryable()
								 .ApplyOrdering(request, this.SearchHandlerFactory)
								 .Where(expression)
								 .ApplyPaging(request);
			}
			else
			{
				return repository.GetQueryable()
								 .Where(expression)
								 .ApplyPaging(request);
			}
		}

		/// <summary>
		/// Invoked when a request processing is completed, allowing for additional handling or modification of the result.
		/// </summary>
		/// <param name="request">The original request that was processed.</param>
		/// <param name="result">The result of the request processing, which can be modified before being returned.</param>
		/// <returns>The potentially modified result of the request processing.</returns>
		protected virtual ControllerActionResult<DataTableResult<TViewModel>> OnRequestCompleted(TRequest request, ControllerActionResult<DataTableResult<TViewModel>> result)
		{
			return result;
		}
	}
}

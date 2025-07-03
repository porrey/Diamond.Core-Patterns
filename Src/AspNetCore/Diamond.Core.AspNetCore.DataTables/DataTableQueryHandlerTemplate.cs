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
	public abstract class DataTableQueryHandlerTemplate<TEntity, TViewModel, TRequest> : IDataTableQueryHandler<TEntity, TViewModel, TRequest>
		where TEntity : IEntity
		where TRequest : IDataTableRequest
	{
		public DataTableQueryHandlerTemplate(ILogger<DataTableQueryHandlerTemplate<TEntity, TViewModel, TRequest>> logger, IRepositoryFactory repositoryFactory, ISearchHandlerFactory<TEntity> searchHandlerFactory, IMapper mapper)
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
			this.SearchHandlerFactory = searchHandlerFactory;
			this.Mapper = mapper;
		}

		protected virtual ILogger<DataTableQueryHandlerTemplate<TEntity, TViewModel, TRequest>> Logger { get; set; }
		protected virtual IRepositoryFactory RepositoryFactory { get; set; }
		protected virtual ISearchHandlerFactory<TEntity> SearchHandlerFactory { get; set; }
		protected virtual IMapper Mapper { get; set; }

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

		protected virtual TRequest OnRequestStarted(TRequest request)
		{
			return request;
		}

		protected virtual Expression<Func<TEntity, bool>> OnGetFilterExpression(TRequest request)
		{
			return request.ApplyFilter<TEntity, TViewModel>(this.SearchHandlerFactory);
		}

		protected virtual Expression<Func<TEntity, bool>> OnGetSearchExpression(TRequest request)
		{
			return request.ApplySearch<TEntity, TViewModel>(this.SearchHandlerFactory);
		}

		protected virtual Expression<Func<TEntity, bool>> OnBuildExpression(Expression<Func<TEntity, bool>> initialExpression, Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, bool>> searchExpression)
		{
			return initialExpression.And(filterExpression).And(searchExpression);
		}

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

		protected virtual ControllerActionResult<DataTableResult<TViewModel>> OnRequestCompleted(TRequest request, ControllerActionResult<DataTableResult<TViewModel>> result)
		{
			return result;
		}
	}
}

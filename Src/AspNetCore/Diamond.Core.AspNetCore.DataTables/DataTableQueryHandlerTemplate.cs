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

		public virtual async Task<IControllerActionResult<DataTableResult<TViewModel>>> ExecuteQueryAsync(TRequest request, Expression<Func<TEntity, bool>> preFilterExpression)
		{
			ControllerActionResult<DataTableResult<TViewModel>> returnValue = new();

			this.OnRequestStarted(request);

			//
			// Get the repository.
			//
			IQueryableRepository<TEntity> repository = await this.RepositoryFactory.GetQueryableAsync<TEntity>();

			//
			// Get the filter expression.
			//
			Expression<Func<TEntity, bool>> filterExpression = this.OnGetFilterExpression(request);

			//
			// Get the search expression.
			//
			Expression<Func<TEntity, bool>> searchExpression = this.OnGetSearchExpression(request);

			//
			// Execute a query
			//
			IEnumerable<TEntity> items = this.OnExecuteQuery(request, preFilterExpression, filterExpression, searchExpression, repository);

			//
			// Set the grid properties.
			//
			returnValue.ResultDetails = DoActionResult.Ok();
			returnValue.Result = new()
			{
				Data = this.Mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(items).ToArray(),
				Draw = request != null ? request.Draw : 1,
				RecordsFiltered = items.Count(),
				RecordsTotal = repository.GetQueryable().Where(preFilterExpression.And(filterExpression)).Count()
			};

			return this.OnRequestCompleted(request, returnValue);
		}

		protected virtual Expression<Func<TEntity, bool>> OnGetFilterExpression(TRequest request)
		{
			return request.ApplyFilter<TEntity, TViewModel>(this.SearchHandlerFactory);
		}

		protected virtual Expression<Func<TEntity, bool>> OnGetSearchExpression(TRequest request)
		{
			return request.ApplySearch<TEntity, TViewModel>(this.SearchHandlerFactory);
		}

		protected virtual IEnumerable<TEntity> OnExecuteQuery(TRequest request, Expression<Func<TEntity, bool>> preFilterExpression, Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, bool>> searchExpression, IQueryableRepository<TEntity> repository)
		{
			return repository.GetQueryable()
							 .ApplyOrdering(request)
							 .Where(preFilterExpression).AsQueryable()
							 .Where(filterExpression).AsQueryable()
							 .Where(searchExpression)
							 .FinalizeQuery(request);
		}

		protected virtual void OnRequestStarted(TRequest request)
		{
		}

		protected virtual ControllerActionResult<DataTableResult<TViewModel>> OnRequestCompleted(TRequest request, ControllerActionResult<DataTableResult<TViewModel>> result)
		{
			return result;
		}
	}
}

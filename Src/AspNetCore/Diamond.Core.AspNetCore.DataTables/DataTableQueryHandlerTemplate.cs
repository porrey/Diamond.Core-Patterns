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

		protected ILogger<DataTableQueryHandlerTemplate<TEntity, TViewModel, TRequest>> Logger { get; set; }
		protected IRepositoryFactory RepositoryFactory { get; set; }
		protected ISearchHandlerFactory<TEntity> SearchHandlerFactory { get; set; }
		protected IMapper Mapper { get; set; }

		public async Task<IControllerActionResult<DataTableResult<TViewModel>>> ExecuteQueryAsync(TRequest request, Expression<Func<TEntity, bool>> preFilterExpression)
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
			Expression<Func<TEntity, bool>> filterExpression = request.ApplyFilter<TEntity, TViewModel>(this.SearchHandlerFactory);

			//
			// Get the search expression.
			//
			Expression<Func<TEntity, bool>> searchExpression = request.ApplySearch<TEntity, TViewModel>(this.SearchHandlerFactory);

			//
			// Initialize a query
			//
			IEnumerable<TEntity> items = repository.GetQueryable()
												   .ApplyOrdering(request)
												   .Where((preFilterExpression).And(filterExpression).And(searchExpression))
												   .FinalizeQuery(request);

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

		public virtual void OnRequestStarted(TRequest request)
		{
		}

		public virtual ControllerActionResult<DataTableResult<TViewModel>> OnRequestCompleted(TRequest request, ControllerActionResult<DataTableResult<TViewModel>> result)
		{
			return result;
		}
	}
}

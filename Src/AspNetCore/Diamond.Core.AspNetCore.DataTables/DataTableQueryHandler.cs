using AutoMapper;
using Diamond.Core.AspNetCore.DataTables;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Lsc.Logistics.BookPortal
{
	public class DataTableQueryHandler<TEntity, TViewModel, TRequest> : DataTableQueryHandlerTemplate<TEntity, TViewModel, TRequest>
		where TEntity : IEntity
		where TRequest : IDataTableRequest
	{
		public DataTableQueryHandler(ILogger<DataTableQueryHandler<TEntity, TViewModel, TRequest>> logger, IRepositoryFactory repositoryFactory, ISearchHandlerFactory<TEntity> searchHandlerFactory, IMapper mapper)
			: base(logger, repositoryFactory, searchHandlerFactory, mapper)
		{
		}
	}
}

using System.Linq.Expressions;
using Diamond.Core.AspNetCore.DoAction;

namespace Diamond.Core.AspNetCore.DataTables
{
	public interface IDataTableQueryHandler<TEntity, TViewModel, TRequest>
	{
		Task<IControllerActionResult<DataTableResult<TViewModel>>> ExecuteQueryAsync(TRequest request, Expression<Func<TEntity, bool>> preFilterExpression);
	}
}

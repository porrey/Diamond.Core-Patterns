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
using Diamond.Core.AspNetCore.DoAction;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Defines a handler for executing data table queries and returning results in a specified view model format.	
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
	/// <typeparam name="TViewModel">The type of the view model to which the query results are mapped.</typeparam>
	/// <typeparam name="TRequest">The type of the request object containing query parameters.</typeparam>
	public interface IDataTableQueryHandler<TEntity, TViewModel, TRequest>
	{
		/// <summary>
		/// Executes an asynchronous query based on the specified request and pre-filter expression.
		/// </summary>
		/// <param name="request">The request object containing query parameters and options.</param>
		/// <param name="preFilterExpression">An expression used to pre-filter the entities before executing the query. Cannot be null.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains an <see
		/// cref="IControllerActionResult{T}"/> with a <see cref="DataTableResult{TViewModel}"/> representing the query
		/// results.</returns>
		Task<IControllerActionResult<DataTableResult<TViewModel>>> ExecuteQueryAsync(TRequest request, Expression<Func<TEntity, bool>> preFilterExpression);
	}
}

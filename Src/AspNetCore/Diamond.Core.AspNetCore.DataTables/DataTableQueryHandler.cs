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
using AutoMapper;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Handles data table queries by processing requests and transforming entities into view models.
	/// </summary>
	/// <remarks>This class extends <see cref="DataTableQueryHandlerTemplate{TEntity, TViewModel,
	/// DataTableRequest}"/> to provide specific functionality for handling data table requests. It utilizes a logger for
	/// logging operations, a repository factory for data access, a search handler factory for processing search queries,
	/// and a mapper for transforming entities into view models.</remarks>
	/// <typeparam name="TEntity">The type of the entity being queried, which must implement <see cref="IEntity"/>.</typeparam>
	/// <typeparam name="TViewModel">The type of the view model to which entities are mapped.</typeparam>
	public class DataTableQueryHandler<TEntity, TViewModel> : DataTableQueryHandlerTemplate<TEntity, TViewModel, DataTableRequest>
		where TEntity : IEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataTableQueryHandler{TEntity, TViewModel}"/> class.
		/// </summary>
		/// <param name="logger">The logger instance used for logging operations within the handler.</param>
		/// <param name="repositoryFactory">The factory used to create repository instances for data access.</param>
		/// <param name="searchHandlerFactory">The factory used to create search handlers for processing search queries.</param>
		/// <param name="mapper">The mapper used for transforming entities to view models.</param>
		public DataTableQueryHandler(ILogger<DataTableQueryHandler<TEntity, TViewModel>> logger, IRepositoryFactory repositoryFactory, ISearchHandlerFactory<TEntity> searchHandlerFactory, IMapper mapper)
			: base(logger, repositoryFactory, searchHandlerFactory, mapper)
		{
		}
	}
}

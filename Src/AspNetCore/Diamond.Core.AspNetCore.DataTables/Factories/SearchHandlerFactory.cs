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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Provides a factory for creating search handlers for a specified entity type.
	/// </summary>
	/// <remarks>This factory utilizes dependency injection to retrieve available search handlers and selects the
	/// appropriate handler based on the specified property name.</remarks>
	/// <typeparam name="TEntity">The type of entity for which the search handlers are created.</typeparam>
	public class SearchHandlerFactory<TEntity> : ISearchHandlerFactory<TEntity>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SearchHandlerFactory{TEntity}"/> class.
		/// </summary>
		/// <param name="logger">The logger instance used for logging operations within the factory.</param>
		/// <param name="serviceProvider">The service provider used to resolve dependencies for search handlers.</param>
		public SearchHandlerFactory(ILogger<SearchHandlerFactory<TEntity>> logger, IServiceProvider serviceProvider)
		{
			this.Logger = logger;
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// Gets or sets the logger used for logging messages related to the <see cref="SearchHandlerFactory{TEntity}"/>
		/// operations.
		/// </summary>
		protected virtual ILogger<SearchHandlerFactory<TEntity>> Logger { get; set; } = new NullLogger<SearchHandlerFactory<TEntity>>();
		
		/// <summary>
		/// Gets or sets the service provider used to resolve service dependencies.
		/// </summary>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// Asynchronously retrieves a search handler for the specified property of the entity.
		/// </summary>
		/// <param name="propertyName">The name of the property for which to retrieve the search handler. Cannot be null or empty.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains an <see cref="ISearchHandler{TEntity}"/>
		/// for the specified property.</returns>
		public virtual Task<ISearchHandler<TEntity>> GetAsync(string propertyName)
		{
			return this.OnGetAsync(propertyName);
		}

		/// <summary>
		/// Asynchronously retrieves a search handler for the specified property name.
		/// </summary>
		/// <param name="propertyName">The name of the property for which to retrieve the search handler. This parameter is case-insensitive.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the <see
		/// cref="ISearchHandler{TEntity}"/> associated with the specified property name, or <see langword="null"/> if no
		/// handler is found.</returns>
		protected virtual Task<ISearchHandler<TEntity>> OnGetAsync(string propertyName)
		{
			ISearchHandler<TEntity> returnValue = null;

			//
			// Get the handlers.
			//
			IEnumerable<ISearchHandler<TEntity>> handlers = this.ServiceProvider.GetService<IEnumerable<ISearchHandler<TEntity>>>();

			//
			// Get the specific handler.
			//
			returnValue = handlers.Where(t => t.PropertyName.ToLower() == propertyName.ToLower()).FirstOrDefault();

			return Task.FromResult(returnValue);
		}
	}
}

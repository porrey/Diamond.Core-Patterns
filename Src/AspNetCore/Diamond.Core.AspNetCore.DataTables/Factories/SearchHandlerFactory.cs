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
	public class SearchHandlerFactory<TEntity> : ISearchHandlerFactory<TEntity>
	{
		public SearchHandlerFactory(ILogger<SearchHandlerFactory<TEntity>> logger, IServiceProvider serviceProvider)
		{
			this.Logger = logger;
			this.ServiceProvider = serviceProvider;
		}

		protected virtual ILogger<SearchHandlerFactory<TEntity>> Logger { get; set; } = new NullLogger<SearchHandlerFactory<TEntity>>();
		protected virtual IServiceProvider ServiceProvider { get; set; }

		public virtual Task<ISearchHandler<TEntity>> GetAsync(string propertyName)
		{
			return this.OnGetAsync(propertyName);
		}

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

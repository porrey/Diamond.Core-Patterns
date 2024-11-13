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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.AspNetCore.DataTables
{
	public abstract class SearchHandlerTemplate<TModel> : ISearchHandler<TModel>
	{
		public SearchHandlerTemplate(ILogger<SearchHandlerTemplate<TModel>> logger)
		{
			this.Logger = logger;
		}

		protected virtual ILogger<SearchHandlerTemplate<TModel>> Logger { get; set; } = new NullLogger<SearchHandlerTemplate<TModel>>();
		public virtual string PropertyName { get; }

		public Task<Expression<Func<TModel, bool>>> ApplySearchFilterAsync(SearchType searchType, string searchTerm)
		{
			Expression<Func<TModel, bool>> returnValue = searchType == SearchType.GlobalSearch ? t => false : t => true;

			try
			{
				this.Logger.LogDebug("{type} search requested for search term '{term}' on property '{name}'.", searchType, searchTerm, this.PropertyName);

				if (searchType == SearchType.GlobalSearch)
				{
					returnValue = this.OnSearch(searchTerm);
				}
				else
				{
					returnValue = this.OnFilter(searchTerm);
				}
			}
			catch (Exception ex)
			{
				this.Logger.LogError(ex, "Exception in SearchHandlerTemplate for '{name}'.", this.PropertyName);
			}

			return Task.FromResult(returnValue);
		}

		protected virtual Expression<Func<TModel, bool>> OnSearch(string searchTerm)
		{
			return null;
		}

		protected virtual Expression<Func<TModel, bool>> OnFilter(string searchTerm)
		{
			return null;
		}
	}
}

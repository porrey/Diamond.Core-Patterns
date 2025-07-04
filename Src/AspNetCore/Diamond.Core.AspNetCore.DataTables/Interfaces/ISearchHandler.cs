﻿//
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

namespace Diamond.Core.AspNetCore.DataTables
{
	public enum SearchType
	{
		GlobalSearch,
		Column
	}

	public interface ISearchHandler<TEntity>
	{
		string PropertyName { get; }
		Task<Expression<Func<TEntity, bool>>> ApplySearchFilterAsync(SearchType searchType, string searchTerm);
		IOrderedQueryable<TEntity> AddOrderBySort(IQueryable<TEntity> source, string columnName, string direction);
		IOrderedQueryable<TEntity> AddThenBySort(IOrderedQueryable<TEntity> source, string columnName, string direction);
	}
}

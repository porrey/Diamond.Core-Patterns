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
	}
}

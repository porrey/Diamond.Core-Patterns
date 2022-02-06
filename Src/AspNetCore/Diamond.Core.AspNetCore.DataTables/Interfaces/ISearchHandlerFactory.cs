namespace Diamond.Core.AspNetCore.DataTables
{
	public interface ISearchHandlerFactory<TEntity>
	{
		Task<ISearchHandler<TEntity>> GetAsync(string propertyName);
	}
}

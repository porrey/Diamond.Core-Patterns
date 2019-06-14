using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IRepository
	{
	}

	public interface IRepository<TInterface> : IRepository where TInterface : IEntity
	{
	}

	//public interface IRepository<TItem> : IRepository
	//{
	//	Task<IEnumerable<TItem>> GetAllAsync();
	//	Task<IEnumerable<TItem>> GetAsync(Func<TItem, bool> predicate);

	//	Task<IRepositoryContext> GetContextAsync();
	//	Task<IQueryable<TItem>> GetQueryableAsync(IRepositoryContext context);
	//	Task<IQueryable<TItem>> GetQueryableAsync(IRepositoryContext context, Func<TItem, bool> predicate);

	//	Task<TItem> CreateEmptyAsync();
	//	Task<bool> AddAsync(TItem item);
	//	Task<bool> UpdateAsync(TItem item);
	//}
}

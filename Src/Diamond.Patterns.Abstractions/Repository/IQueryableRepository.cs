using System;
using System.Linq;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IQueryableRepository<TInterface> : IReadOnlyRepository<TInterface> where TInterface : IEntity
	{
		Task<IRepositoryContext> GetContextAsync();
		Task<IQueryable<TInterface>> GetQueryableAsync(IRepositoryContext context);
		Task<IQueryable<TInterface>> GetQueryableAsync(IRepositoryContext context, Func<TInterface, bool> predicate);
	}
}

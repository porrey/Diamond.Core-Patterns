using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IReadOnlyRepository<TInterface> : IRepository<TInterface> where TInterface : IEntity
	{
		Task<IEnumerable<TInterface>> GetAllAsync();
		Task<IEnumerable<TInterface>> GetAsync(Func<TInterface, bool> predicate);
	}
}

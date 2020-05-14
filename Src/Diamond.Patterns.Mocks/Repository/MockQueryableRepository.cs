using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Mocks
{
	public class MockQueryableRepository<TInterface> : MockReadOnlyRepository<TInterface>, IQueryableRepository<TInterface>
		where TInterface : IEntity
	{
		public MockQueryableRepository(IEnumerable<TInterface> items)
			: base(items)
		{
		}

		public Task<IRepositoryContext> GetContextAsync()
		{
			return Task.FromResult<IRepositoryContext>(new MockRepositoryContext());
		}

		public Task<IQueryable<TInterface>> GetQueryableAsync(IRepositoryContext context)
		{
			return Task.FromResult(this.Items.AsQueryable());
		}
	}
}

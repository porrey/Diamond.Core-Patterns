using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Mocks
{
	public class MockWritableRepository<TInterface> : MockQueryableRepository<TInterface>, IWritableRepository<TInterface>
		where TInterface : IEntity
	{
		public MockWritableRepository(IEnumerable<TInterface> items)
			: base(items)
		{
		}

		public IEntityFactory<TInterface> ModelFactory { get; }

		public Task<(bool, TInterface)> AddAsync(TInterface entity)
		{
			this.Items.Add(entity);
			return Task.FromResult((true, entity));
		}

		public Task<TInterface> AddAsync(IRepositoryContext repositoryContext, TInterface entity)
		{
			this.Items.Add(entity);
			return Task.FromResult(entity);
		}

		public async Task<bool> DeleteAsync(TInterface entity)
		{
			var item = (await this.GetAsync(t => t.Equals(entity))).Single();
			return true;
		}

		public async Task<bool> DeleteAsync(IRepositoryContext repositoryContext, TInterface entity)
		{
			var item = (await this.GetAsync(t => t.Equals(entity))).Single();
			return true;
		}

		public async Task<bool> UpdateAsync(TInterface entity)
		{
			await this.DeleteAsync(entity);
			await this.AddAsync(entity);
			return true;
		}

		public async Task<bool> UpdateAsync(IRepositoryContext repositoryContext, TInterface entity)
		{
			await this.DeleteAsync(entity);
			await this.AddAsync(entity);
			return true;
		}
	}
}

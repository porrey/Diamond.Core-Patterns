using System.Collections.Generic;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Mocks
{
	public class MockRepositoryFactory : IRepositoryFactory
	{
		public MockRepositoryFactory(IEnumerable<IRepository> items)
		{
			this.Items = items;
		}

		public IEnumerable<IRepository> Items { get; set; }

		public Task<IRepository<TInterface>> GetAsync<TInterface>() where TInterface : IEntity
		{
			IRepository<TInterface> returnValue = null;

			foreach (var item in this.Items)
			{
				if (item is IRepository<TInterface>)
				{
					returnValue = (IRepository<TInterface>)item;
					break;
				}
			}

			return Task.FromResult(returnValue);
		}

		public Task<IRepository<TInterface>> GetAsync<TInterface>(string name = null) where TInterface : IEntity
		{
			return this.GetAsync<TInterface>();
		}

		public async Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>() where TInterface : IEntity
		{
			return (await this.GetAsync<TInterface>()) as IQueryableRepository<TInterface>;
		}

		public async Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>(string name = null) where TInterface : IEntity
		{
			return (await this.GetAsync<TInterface>()) as IQueryableRepository<TInterface>;
		}

		public async Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>() where TInterface : IEntity
		{
			return (await this.GetAsync<TInterface>()) as IReadOnlyRepository<TInterface>;
		}

		public async Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>(string name = null) where TInterface : IEntity
		{
			return (await this.GetAsync<TInterface>()) as IReadOnlyRepository<TInterface>;
		}

		public async Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>() where TInterface : IEntity
		{
			return (await this.GetAsync<TInterface>()) as IWritableRepository<TInterface>;
		}

		public async Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>(string name = null) where TInterface : IEntity
		{
			return (await this.GetAsync<TInterface>()) as IWritableRepository<TInterface>;
		}
	}
}

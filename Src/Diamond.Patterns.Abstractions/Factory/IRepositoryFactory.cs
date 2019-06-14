using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IRepositoryFactory
	{
		Task<IRepository<TInterface>> GetAsync<TInterface>() where TInterface : IEntity;
		Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>() where TInterface : IEntity;
		Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>() where TInterface : IEntity;
		Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>() where TInterface : IEntity;
	}
}

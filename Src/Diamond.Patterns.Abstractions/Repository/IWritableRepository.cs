using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWritableRepository<TInterface> : IQueryableRepository<TInterface> where TInterface : IEntity
	{
		IEntityFactory<TInterface> ModelFactory { get; }
		Task<bool> AddAsync(TInterface entity);
		Task<bool> DeleteAsync(TInterface entity);
		Task<bool> UpdateAsync(TInterface entity);
	}
}

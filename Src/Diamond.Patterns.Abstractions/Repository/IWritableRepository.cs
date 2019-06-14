using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWritableRepository<TInterface> : IQueryableRepository<TInterface> where TInterface : IEntity
	{
		IEntityFactory<TInterface> ModelFactory { get; }
		Task<bool> AddAsync(TInterface industry);
		Task<bool> DeleteAsync(TInterface model);
		Task<bool> UpdateAsync(TInterface model);
	}
}

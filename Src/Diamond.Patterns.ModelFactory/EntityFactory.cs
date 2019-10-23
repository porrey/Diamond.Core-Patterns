using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.ModelFactory
{
	public class EntityFactory<TInterface, TEntity> : IEntityFactory<TInterface>
		where TEntity : TInterface, new()
		where TInterface : IEntity
	{
		public Task<TInterface> CreateAsync()
		{
			return Task.FromResult<TInterface>(new TEntity());
		}
	}
}

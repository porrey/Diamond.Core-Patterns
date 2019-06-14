using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IEntityFactory<TInterface> where TInterface : IEntity
	{
		Task<TInterface> CreateAsync();
	}
}

namespace Diamond.Patterns.Abstractions
{
	public interface IRepository
	{
	}

	public interface IRepository<TInterface> : IRepository where TInterface : IEntity
	{
	}
}

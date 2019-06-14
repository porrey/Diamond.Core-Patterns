namespace Diamond.Patterns.Abstractions
{
	public interface IEntity
	{
	}

	public interface IEntity<TKey> : IEntity
	{
		TKey Id { get; set; }
	}
}

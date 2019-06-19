namespace Diamond.Patterns.Abstractions
{
	public interface IRepositoryConfiguration
	{
		string Description { get; }
		string ConnectionString { get; }
	}
}

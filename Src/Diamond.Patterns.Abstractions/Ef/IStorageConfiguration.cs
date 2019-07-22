namespace Diamond.Patterns.Abstractions
{
	public interface IStorageConfiguration
	{
		string Description { get; }
		string ConnectionString { get; }
	}
}

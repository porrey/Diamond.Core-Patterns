using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.DatabaseStrategy.SQLite
{
	public class ConfigFileRepositoryConfiguration : IStorageConfiguration
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ConnectionString => $"name={this.Name}";
	}
}

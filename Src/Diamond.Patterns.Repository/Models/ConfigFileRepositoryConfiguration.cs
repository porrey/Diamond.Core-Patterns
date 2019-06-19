using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Repository
{
	public class ConfigFileRepositoryConfiguration : IRepositoryConfiguration
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ConnectionString => $"name={this.Name}";
	}
}

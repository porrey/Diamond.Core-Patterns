using Diamond.Patterns.Abstractions;

namespace Lsc.Logistics.Patterns.Repository.SqLite
{
	public class ConfigFileRepositoryConfiguration : IRepositoryConfiguration
	{
		public string Name { get; set; }
		public string ConnectionString => this.Name;
	}
}

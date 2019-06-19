using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Repository
{
	public class DataSourceRepositoryConfiguration : IRepositoryConfiguration
	{
		public string DataSource { get; set; }
		public string Description { get; set; }
		public string ConnectionString => this.DataSource;
	}
}

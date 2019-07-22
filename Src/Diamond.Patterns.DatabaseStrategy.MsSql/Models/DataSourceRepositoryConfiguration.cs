using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.DatabaseStrategy.MsSql
{
	public class DataSourceRepositoryConfiguration : IStorageConfiguration
	{
		public string DataSource { get; set; }
		public string Description { get; set; }
		public string ConnectionString => this.DataSource;
	}
}

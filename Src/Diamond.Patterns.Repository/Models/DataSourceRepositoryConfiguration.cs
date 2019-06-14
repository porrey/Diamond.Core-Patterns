using Diamond.Patterns.Abstractions;

namespace Lsc.Logistics.Patterns.Repository.SqLite
{
	public class DataSourceRepositoryConfiguration : IRepositoryConfiguration
	{
		public string DataSource { get; set; }
		public string ConnectionString => this.DataSource;
	}
}

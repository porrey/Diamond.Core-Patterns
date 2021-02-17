namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public class DatabaseDescriptorConfiguration : ServiceDescriptorConfiguration
	{
		/// <summary>
		/// 
		/// </summary>
		public string Context
		{
			get
			{
				return this.ImplementationType;
			}
			set
			{
				this.ImplementationType = value;
				this.ServiceType = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string ConnectionString { get; set; }

		///
		public string Factory { get; set; }
	}
}

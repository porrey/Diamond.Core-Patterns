using System;

namespace Diamond.Core.Extensions.JsonServices
{
	/// <summary>
	/// 
	/// </summary>
	public class DependencyFactory
	{
		public DependencyFactory(Type implementationType, ServiceDescriptorConfiguration configuration)
		{
			this.ImplementationType = implementationType;
			this.Configuration = configuration;
		}

		/// <summary>
		/// 
		/// </summary>
		public Type ImplementationType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ServiceDescriptorConfiguration Configuration { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object GetInstance()
		{
			return null;
		}
	}
}

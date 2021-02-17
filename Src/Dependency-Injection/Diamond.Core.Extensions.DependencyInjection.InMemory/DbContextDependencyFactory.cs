using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.DependencyInjection.InMemory
{
	/// <summary>
	/// 
	/// </summary>
	public class DbContextDependencyFactory : IDependencyFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="implementationType"></param>
		/// <param name="configuration"></param>
		public DbContextDependencyFactory(Type implementationType, ServiceDescriptorConfiguration configuration)
		{
			this.ImplementationType = implementationType;
			this.Configuration = configuration;
		}

		/// <summary>
		/// 
		/// </summary>
		public ServiceDescriptorConfiguration Configuration { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Type ImplementationType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sp"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public object GetInstance(IServiceProvider sp, params object[] parameters)
		{
			DbContext context = null;

			try
			{
				DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
				builder.UseInMemoryDatabase((string)parameters[0]);
				context = (DbContext)ActivatorUtilities.CreateInstance(sp, this.ImplementationType, builder.Options);
			}
			catch
			{
				throw new DbContextFactoryException(this.ImplementationType.FullName);
			}

			return context;
		}
	}
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.DependencyInjection.EntityFrameworkCore
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseDbContextDependencyFactory : IDependencyFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="implementationType"></param>
		/// <param name="configuration"></param>
		public BaseDbContextDependencyFactory(Type implementationType, ServiceDescriptorConfiguration configuration)
		{
			this.ImplementationType = implementationType;
			this.Configuration = configuration;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual ServiceDescriptorConfiguration Configuration { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual Type ImplementationType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sp"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual object GetInstance(IServiceProvider sp, params object[] parameters)
		{
			DbContext context = null;

			try
			{
				DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
				this.OnDbContextOptionsBuilder(builder, parameters);
				context = (DbContext)ActivatorUtilities.CreateInstance(sp, this.ImplementationType, builder.Options);
			}
			catch
			{
				throw new DbContextFactoryException(this.ImplementationType.FullName);
			}

			return context;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="parameters"></param>
		protected virtual void OnDbContextOptionsBuilder(DbContextOptionsBuilder builder, object[] parameters)
		{

		}
	}
}

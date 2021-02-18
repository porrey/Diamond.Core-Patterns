using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.DependencyInjection.EntityFrameworkCore
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseDbContextDependencyFactory<TContext> : BaseDbContextDependencyFactory
		where TContext : DbContext
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="implementationType"></param>
		/// <param name="configuration"></param>
		public BaseDbContextDependencyFactory(Type implementationType, ServiceDescriptorConfiguration configuration)
			: base(implementationType, configuration)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sp"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public override object GetInstance(IServiceProvider sp, params object[] parameters)
		{
			DbContext context = null;

			try
			{
				DbContextOptionsBuilder<TContext> builder = new DbContextOptionsBuilder<TContext>();
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
		protected virtual void OnDbContextOptionsBuilder(DbContextOptionsBuilder<TContext> builder, object[] parameters)
		{
		}
	}
}

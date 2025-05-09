﻿//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
				DbContextOptionsBuilder<TContext> builder = new();
				this.OnDbContextOptionsBuilder(builder, parameters);
				builder.UseApplicationServiceProvider(sp);

				if (this.EnableDetailedErrors)
				{
					builder.EnableDetailedErrors();
				}

				if (this.EnableSensitiveDataLogging)
				{
					builder.EnableSensitiveDataLogging();
				}

				if (this.UseLoggerFactory)
				{
					builder.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
				}

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

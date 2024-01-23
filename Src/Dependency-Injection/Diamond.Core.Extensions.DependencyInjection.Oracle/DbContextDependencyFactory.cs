//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.Extensions.DependencyInjection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Extensions.DependencyInjection.Oracle
{
	/// <summary>
	/// 
	/// </summary>
	public class DbContextDependencyFactory : BaseDbContextDependencyFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="implementationType"></param>
		/// <param name="configuration"></param>
		public DbContextDependencyFactory(Type implementationType, ServiceDescriptorConfiguration configuration)
			: base(implementationType, configuration)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="parameters"></param>
		protected override void OnDbContextOptionsBuilder(DbContextOptionsBuilder builder, object[] parameters)
		{
			//
			// Parameters:
			// 1. Connection String
			// 2. Timeout in seconds
			//
			if (parameters.Length == 1)
			{
				builder.UseOracle((string)parameters[0]);
			}
			else
			{
				builder.UseOracle((string)parameters[0], options =>
				{
					options.CommandTimeout((int)parameters[1]);
				});
			}
		}
	}
}

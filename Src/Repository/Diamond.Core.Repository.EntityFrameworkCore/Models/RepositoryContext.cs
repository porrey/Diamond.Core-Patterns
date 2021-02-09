//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Core.Repository.EntityFrameworkCore {
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	public abstract class RepositoryContext<TContext> : DbContext, IRepositoryContext
		where TContext : DbContext {
		/// <summary>
		/// 
		/// </summary>
		public RepositoryContext()
			: base() {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="options"></param>
		public RepositoryContext(DbContextOptions options)
			: base(options) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Task<int> SaveAsync() {
			return this.SaveChangesAsync();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Task<bool> EnsureCreated() {
			return this.Database.EnsureCreatedAsync();
		}
	}
}
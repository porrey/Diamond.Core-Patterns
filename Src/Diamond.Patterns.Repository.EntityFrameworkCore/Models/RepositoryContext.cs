// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using System.Data;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Patterns.Repository.EntityFrameworkCore
{
	public abstract class RepositoryContext<TContext> : DbContext, IRepositoryContext
		where TContext : DbContext
	{
		public RepositoryContext()
			: base()
		{
		}

		public virtual Task<IRepositoryTransactionContext> BeginTransactionAsync(ContextIsolationLevel isolationLevel)
		{
			IsolationLevel localIsolationLevel = RepositoryContext<TContext>.TranslatioIsolationLevel(isolationLevel);
			return Task.FromResult<IRepositoryTransactionContext>(new RepositoryTransactionContext(this.Database.BeginTransaction()));
		}

		public virtual Task<IRepositoryTransactionContext> BeginTransactionAsync()
		{
			return Task.FromResult<IRepositoryTransactionContext>(new RepositoryTransactionContext(this.Database.BeginTransaction()));
		}

		public virtual Task<int> SaveAsync()
		{
			return this.SaveChangesAsync();
		}

		public virtual Task<bool> UseTransactionAsync(IRepositoryTransactionContext transactionContext)
		{
			throw new NotSupportedException();
		}

		public static IsolationLevel TranslatioIsolationLevel(ContextIsolationLevel isolationLevel)
		{
			IsolationLevel returnValue = IsolationLevel.Chaos;

			switch (isolationLevel)
			{
				case ContextIsolationLevel.ReadStability:
					returnValue = IsolationLevel.ReadCommitted;
					break;
				case ContextIsolationLevel.RepeatableRead:
					returnValue = IsolationLevel.RepeatableRead;
					break;
				case ContextIsolationLevel.UncommitedRead:
					returnValue = IsolationLevel.ReadUncommitted;
					break;
				case ContextIsolationLevel.CursorStability:
					returnValue = IsolationLevel.Snapshot;
					break;
			}

			return returnValue;
		}

		public virtual Task EnableBulkLoadAsync()
		{
			return Task.FromResult(0);
		}

		public  Task DisableBulkLoadAsync()
		{
			return Task.FromResult(0);
		}

		public virtual Task<int> ExecuteSqlCommandAsync(string sql)
		{
			throw new NotSupportedException();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}
	}
}
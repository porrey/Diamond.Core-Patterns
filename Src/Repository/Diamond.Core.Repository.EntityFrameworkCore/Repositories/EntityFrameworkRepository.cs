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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Repository.EntityFrameworkCore
{
	/// <summary>
	/// This repository implements a base repository for an Entity (TEntity) that
	/// implements interface TItem.
	/// </summary>
	/// <typeparam name="TInterface">The interface type that the entity implements.</typeparam>
	/// <typeparam name="TEntity">The entity object type.</typeparam>
	/// <typeparam name="TContext">The Entity Framework database context.</typeparam>
	public abstract class EntityFrameworkRepository<TInterface, TEntity, TContext> : IWritableRepository<TInterface>, IQueryableRepository<TInterface>, IReadOnlyRepository<TInterface>
		where TEntity : class, TInterface, new()
		where TInterface : IEntity
		where TContext : DbContext
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="context"></param>
		/// <param name="modelFactory"></param>
		public EntityFrameworkRepository(ILogger<EntityFrameworkRepository<TInterface, TEntity, TContext>> logger, TContext context, IEntityFactory<TInterface> modelFactory)
		{
			this.Logger = logger;
			this.Context = context;
			this.ModelFactory = modelFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="modelFactory"></param>
		public EntityFrameworkRepository(TContext context, IEntityFactory<TInterface> modelFactory)
		{
			this.Context = context;
			this.ModelFactory = modelFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected TContext Context { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		protected abstract DbSet<TEntity> MyDbSet(TContext model);

		/// <summary>
		/// 
		/// </summary>
		public virtual IEntityFactory<TInterface> ModelFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ILogger<EntityFrameworkRepository<TInterface, TEntity, TContext>> Logger { get; set; } = new NullLogger<EntityFrameworkRepository<TInterface, TEntity, TContext>>();

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Task<IEnumerable<TInterface>> GetAllAsync()
		{
			IEnumerable<TInterface> returnValue = null;

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetAllAsync), typeof(TInterface).Name);
			returnValue = this.MyDbSet(this.Context).AsNoTracking().ToArray();

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public virtual Task<IEnumerable<TInterface>> GetAsync(Expression<Func<TInterface, bool>> predicate)
		{
			IEnumerable<TInterface> returnValue = null;

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetAsync), typeof(TInterface).Name);
			returnValue = this.MyDbSet(this.Context).AsNoTracking().Where(predicate).ToArray();

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Task<IRepositoryContext> GetContextAsync()
		{
			this.Logger.LogDebug("{method} called.", nameof(GetContextAsync));
			return Task.FromResult((IRepositoryContext)this.Context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual Task<IQueryable<TInterface>> GetQueryableAsync(IRepositoryContext context)
		{
			IQueryable<TInterface> returnValue = null;

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetQueryableAsync), typeof(TInterface).Name);

			if (context is TContext db)
			{
				returnValue = this.MyDbSet(this.Context).AsQueryable<TInterface>();
			}
			else
			{
				throw new InvalidContextException();
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual async Task<bool> UpdateAsync(TInterface item)
		{
			bool returnValue = false;

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(UpdateAsync), typeof(TInterface).Name);

			this.Context.Entry((TEntity)item).State = EntityState.Modified;
			int result = await this.Context.SaveChangesAsync(true);
			this.Logger.LogDebug("{method}: Records updated = {result}.", nameof(UpdateAsync), result);
			returnValue = (result == 1);

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual async Task<(bool, TInterface)> AddAsync(TInterface item)
		{
			(bool result, TInterface entity) = (false, default);

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(AddAsync), typeof(TInterface).Name);

			entity = this.MyDbSet(this.Context).Add((TEntity)item).Entity;
			result = (await this.Context.SaveChangesAsync(true) == 1);
			this.Logger.LogDebug("{method}: Records updated = {result}.", nameof(AddAsync), result);

			return (result, entity);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual async Task<bool> DeleteAsync(TInterface item)
		{
			bool returnValue = false;

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(DeleteAsync), typeof(TInterface).Name);

			this.Context.Entry((TEntity)item).State = EntityState.Deleted;
			int result = await this.Context.SaveChangesAsync(true);
			this.Logger.LogDebug("{method}: Records updated = {result}.", nameof(DeleteAsync), result);
			returnValue = (result == 1);

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="repositoryContext"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<bool> UpdateAsync(IRepositoryContext repositoryContext, TInterface item)
		{
			this.Logger.LogDebug("{method} called for type '{name}' with context.", nameof(UpdateAsync), typeof(TInterface).Name);
			((TContext)repositoryContext).Entry((TEntity)item).State = EntityState.Modified;
			return Task.FromResult(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="repositoryContext"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<TInterface> AddAsync(IRepositoryContext repositoryContext, TInterface item)
		{
			this.Logger.LogDebug("{method} called for type '{name}' with context.", nameof(AddAsync), typeof(TInterface).Name);
			return Task.FromResult<TInterface>(this.MyDbSet((TContext)repositoryContext).Add((TEntity)item).Entity);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="repositoryContext"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<bool> DeleteAsync(IRepositoryContext repositoryContext, TInterface item)
		{
			this.Logger.LogDebug("{method} called for type '{name}' with context.", nameof(DeleteAsync), typeof(TInterface).Name);
			((TContext)repositoryContext).Entry((TEntity)item).State = EntityState.Deleted;
			return Task.FromResult(true);
		}
	}
}

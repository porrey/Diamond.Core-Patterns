//
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
	public abstract class EntityFrameworkRepository<TInterface, TEntity, TContext> : DisposableObject, IWritableRepository<TInterface>, IQueryableRepository<TInterface>, IReadOnlyRepository<TInterface>
		where TEntity : class, TInterface, new()
		where TInterface : IEntity
		where TContext : DbContext, IRepositoryContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityFrameworkRepository{TInterface, TEntity, TContext}"/> class
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="context"></param>
		/// <param name="modelFactory"></param>
		public EntityFrameworkRepository(ILogger<EntityFrameworkRepository<TInterface, TEntity, TContext>> logger, TContext context, IEntityFactory<TInterface> modelFactory)
		{
			this.Name = this.GetType().Name.Replace("Repository", "");
			this.Logger = logger;
			this.Context = context;
			this.ModelFactory = modelFactory;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityFrameworkRepository{TInterface, TEntity, TContext}"/> class
		/// </summary>
		/// <param name="context"></param>
		/// <param name="modelFactory"></param>
		public EntityFrameworkRepository(TContext context, IEntityFactory<TInterface> modelFactory)
		{
			this.Name = this.GetType().Name.Replace("Repository", "");
			this.Context = context;
			this.ModelFactory = modelFactory;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityFrameworkRepository{TInterface, TEntity, TContext}"/> class
		/// </summary>
		/// <param name="modelFactory"></param>
		public EntityFrameworkRepository(IEntityFactory<TInterface> modelFactory)
		{
			this.Name = this.GetType().Name.Replace("Repository", "");
			this.Context = null;
			this.ModelFactory = modelFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual TContext Context { get; set; }

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
		public virtual ILogger<EntityFrameworkRepository<TInterface, TEntity, TContext>> Logger { get; set; } = new NullLogger<EntityFrameworkRepository<TInterface, TEntity, TContext>>();

		/// <summary>
		/// 
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual async Task<IEnumerable<TInterface>> GetAllAsync()
		{
			IEnumerable<TInterface> returnValue = null;

			if (this.Context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetAllAsync), typeof(TInterface).Name);
			returnValue = await this.MyDbSet(this.Context).AsNoTracking().ToArrayAsync();

			return returnValue;
		}

		/// <summary>
		/// Asynchronously retrieves all entities of type <typeparamref name="TInterface"/> from the database.
		/// </summary>
		/// <remarks>The entities are retrieved without tracking, which means changes to the entities will not be
		/// tracked by the context.</remarks>
		/// <param name="context">The repository context used to access the database. Must not be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all
		/// entities of type <typeparamref name="TInterface"/>.</returns>
		public virtual async Task<IEnumerable<TInterface>> GetAllAsync(IRepositoryContext context)
		{
			IEnumerable<TInterface> returnValue = null;

			if (context is not TContext db || context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetAllAsync), typeof(TInterface).Name);
			returnValue = await this.MyDbSet((TContext)context).AsNoTracking().ToArrayAsync();

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public virtual async Task<IEnumerable<TInterface>> GetAsync(Expression<Func<TInterface, bool>> predicate)
		{
			IEnumerable<TInterface> returnValue = null;

			if (this.Context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetAsync), typeof(TInterface).Name);
			returnValue = await this.MyDbSet(this.Context).AsNoTracking().Where(predicate).ToArrayAsync();

			return returnValue;
		}

		/// <summary>
		/// Asynchronously retrieves a collection of entities that match the specified predicate from the data source.
		/// </summary>
		/// <remarks>The method performs a query against the data source without tracking the retrieved entities in
		/// the context, which is suitable for read-only operations.</remarks>
		/// <param name="context">The repository context used to access the data source. Must not be null.</param>
		/// <param name="predicate">An expression to filter the entities. Only entities that satisfy this condition will be included in the result.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of entities
		/// that match the specified predicate.</returns>
		public virtual async Task<IEnumerable<TInterface>> GetAsync(IRepositoryContext context, Expression<Func<TInterface, bool>> predicate)
		{
			IEnumerable<TInterface> returnValue = null;

			if (context is not TContext db || context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetAsync), typeof(TInterface).Name);
			returnValue = await this.MyDbSet((TContext)context).AsNoTracking().Where(predicate).ToArrayAsync();

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Task<IRepositoryContext> GetContextAsync()
		{
			this.Logger.LogDebug("{method} called.", nameof(GetContextAsync));
			return Task.FromResult(this.GetContext());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual IRepositoryContext GetContext()
		{
			if (this.Context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called.", nameof(GetContext));
			return (IRepositoryContext)this.Context;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Task<IQueryable<TInterface>> GetQueryableAsync()
		{
			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetQueryableAsync), typeof(TInterface).Name);
			return Task.FromResult(this.GetQueryable());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual Task<IQueryable<TInterface>> GetQueryableAsync(IRepositoryContext context)
		{
			if (context is not TContext db || context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetQueryableAsync), typeof(TInterface).Name);
			return Task.FromResult(this.GetQueryable(context));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual IQueryable<TInterface> GetQueryable()
		{
			IQueryable<TInterface> returnValue = null;

			if (this.Context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetQueryable), typeof(TInterface).Name);
			returnValue = this.MyDbSet(this.Context).AsQueryable<TInterface>();

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual IQueryable<TInterface> GetQueryable(IRepositoryContext context)
		{
			IQueryable<TInterface> returnValue = null;

			if (context is not TContext db || context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(GetQueryable), typeof(TInterface).Name);
			returnValue = this.MyDbSet(db).AsQueryable<TInterface>();

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<int> UpdateAsync(TInterface item)
		{
			if (this.Context == null)
			{
				throw new InvalidContextException();
			}

			return this.UpdateAsync(this.Context, item, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="item"></param>
		/// <param name="commit"></param>
		/// <returns></returns>
		public virtual async Task<int> UpdateAsync(IRepositoryContext context, TInterface item, bool commit = true)
		{
			int returnValue = 0;

			if (context is not TContext db || context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(UpdateAsync), typeof(TInterface).Name);

			db.Entry((TEntity)item).State = EntityState.Modified;
			int result = 0;

			if (commit)
			{
				returnValue = await db.SaveChangesAsync(false);
				db.ChangeTracker.Clear();
				this.Logger.LogDebug("{method}: Records updated = {result}.", nameof(UpdateAsync), result);
			}
			else
			{
				returnValue = 1;
				this.Logger.LogDebug("{method}: Record marked for update.", nameof(UpdateAsync));
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<(int, TInterface)> AddAsync(TInterface item)
		{
			if (this.Context == null)
			{
				throw new InvalidContextException();
			}

			return this.AddAsync(this.Context, item, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="item"></param>
		/// <param name="commit"></param>
		/// <returns></returns>
		public virtual async Task<(int, TInterface)> AddAsync(IRepositoryContext context, TInterface item, bool commit = true)
		{
			(int result, TInterface entity) = (0, default);

			if (context is not TContext db || context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(AddAsync), typeof(TInterface).Name);
			entity = db.Add((TEntity)item).Entity;

			if (commit)
			{
				result = await db.SaveChangesAsync(true);
				db.ChangeTracker.Clear();
				this.Logger.LogDebug("{method}: Records updated = {result}.", nameof(AddAsync), result);
			}
			else
			{
				entity = item;
				result = 1;
				this.Logger.LogDebug("{method}: Record marked for addition.", nameof(AddAsync));
			}

			return (result, entity);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<int> DeleteAsync(TInterface item)
		{
			if (this.Context == null)
			{
				throw new InvalidContextException();
			}

			return this.DeleteAsync(this.Context, item, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="item"></param>
		/// <param name="commit"></param>
		/// <returns></returns>
		public virtual async Task<int> DeleteAsync(IRepositoryContext context, TInterface item, bool commit = true)
		{
			int returnValue = 0;

			if (context is not TContext db || context == null)
			{
				throw new InvalidContextException();
			}

			this.Logger.LogDebug("{method} called for type '{name}'.", nameof(DeleteAsync), typeof(TInterface).Name);
			db.Entry((TEntity)item).State = EntityState.Deleted;
			int result = 0;

			if (commit)
			{
				returnValue = await db.SaveChangesAsync(true);
				db.ChangeTracker.Clear();
				this.Logger.LogDebug("{method}: Records updated = {result}.", nameof(DeleteAsync), result);
			}
			else
			{
				returnValue = 1;
				this.Logger.LogDebug("{method}: Records marked for deletion.", nameof(DeleteAsync));
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDisposeManagedObjects()
		{
			this.Logger.LogDebug("Disposed {name}", nameof(EntityFrameworkRepository<TInterface, TEntity, TContext>));

			if (this.Context != null)
			{
				this.Context.Dispose();
			}

			base.OnDisposeManagedObjects();
		}
	}
}

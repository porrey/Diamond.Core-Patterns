using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Repository.EntityFramework
{
	/// <summary>
	/// This repository implements a base repository for an Entity (TEntity) that
	/// implements interface TItem.
	/// </summary>
	/// <typeparam name="TInterface">The interface type that the entity implements.</typeparam>
	/// <typeparam name="TEntity">The entity object type.</typeparam>
	public abstract class EntityFrameworkRepository<TInterface, TEntity> : IWritableRepository<TInterface>
		where TEntity : class, TInterface, new()
		where TInterface : IEntity
	{
		public EntityFrameworkRepository(IRepositoryConfiguration repositoryConfiguration, IEntityFactory<TInterface> modelFactory)
		{
			this.RepositoryConfiguration = repositoryConfiguration;
			this.ModelFactory = modelFactory;
		}

		protected IRepositoryConfiguration RepositoryConfiguration { get; set; }
		protected abstract DbSet<TEntity> MyDbSet(DbContext model);
		protected abstract DbContext GetNewDbContext { get; }
		public IEntityFactory<TInterface> ModelFactory { get; set; }

		public virtual async Task<IEnumerable<TInterface>> GetAllAsync()
		{
			return await this.OnGetAllAsync();
		}

		public virtual async Task<IEnumerable<TInterface>> GetAsync(Func<TInterface, bool> predicate)
		{
			return await this.OnGetAsync(predicate);
		}

		public Task<IRepositoryContext> GetContextAsync()
		{
			return Task.FromResult((IRepositoryContext)this.GetNewDbContext);
		}

		public async Task<IQueryable<TInterface>> GetQueryableAsync(IRepositoryContext context)
		{
			return await this.OnGetQueryableAsync(context);
		}

		public async Task<IQueryable<TInterface>> GetQueryableAsync(IRepositoryContext context, Func<TInterface, bool> predicate)
		{
			return await this.OnGetQueryableAsync(context, predicate);
		}

		public virtual async Task<TInterface> CreateEmptyAsync()
		{
			return await this.OnCreateEmptyAsync();
		}

		public virtual async Task<bool> UpdateAsync(TInterface item)
		{
			return await this.OnUpdateAsync(item);
		}

		public virtual async Task<bool> AddAsync(TInterface item)
		{
			return await this.OnAddAsync(item);
		}

		public virtual async Task<bool> DeleteAsync(TInterface item)
		{
			return await this.OnDeleteAsync(item);
		}

		#region Protected Members
		protected virtual async Task<bool> OnUpdateAsync(TInterface item)
		{
			bool returnValue = false;

			using (DbContext db = this.GetNewDbContext)
			{
				db.Entry((TEntity)item).State = EntityState.Modified;
				int result = await db.SaveChangesAsync();
				returnValue = (result == 1);
			}

			return returnValue;
		}

		protected virtual Task<TInterface> OnCreateEmptyAsync()
		{
			return Task.FromResult((TInterface)new TEntity());
		}

		protected virtual async Task<bool> OnAddAsync(TInterface item)
		{
			bool returnValue = false;

			using (DbContext db = this.GetNewDbContext)
			{
				this.MyDbSet(db).Add((TEntity)item);
				int result = await db.SaveChangesAsync();
				returnValue = (result == 1);
			}

			return returnValue;
		}

		protected virtual async Task<bool> OnDeleteAsync(TInterface item)
		{
			bool returnValue = false;

			using (DbContext db = this.GetNewDbContext)
			{
				db.Entry((TEntity)item).State = EntityState.Deleted;
				int result = await db.SaveChangesAsync();
				returnValue = (result == 1);
			}

			return returnValue;
		}

		protected virtual Task<IEnumerable<TInterface>> OnGetAllAsync()
		{
			IEnumerable<TInterface> returnValue = null;

			using (DbContext db = this.GetNewDbContext)
			{
				returnValue = this.MyDbSet(db).AsNoTracking().ToArray();
			}

			return Task.FromResult(returnValue);
		}

		protected virtual Task<IEnumerable<TInterface>> OnGetAsync(Func<TInterface, bool> predicate)
		{
			IEnumerable<TInterface> returnValue = null;

			using (DbContext db = this.GetNewDbContext)
			{
				returnValue = this.MyDbSet(db).AsNoTracking().Where(predicate).ToArray();
			}

			return Task.FromResult(returnValue);
		}

		protected virtual Task<IQueryable<TInterface>> OnGetQueryableAsync(IRepositoryContext context)
		{
			DbContext db = (DbContext)context;
			return Task.FromResult(this.MyDbSet(db).AsQueryable<TInterface>());
		}

		protected virtual Task<IQueryable<TInterface>> OnGetQueryableAsync(IRepositoryContext context, Func<TInterface, bool> predicate)
		{
			DbContext db = (DbContext)context;
			return Task.FromResult(this.MyDbSet(db).Where(predicate).AsQueryable<TInterface>());
		}
		#endregion
	}
}

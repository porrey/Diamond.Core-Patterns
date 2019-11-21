using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Repository
{
	internal class RepositoryFactory : IRepositoryFactory
	{
		public RepositoryFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		protected IObjectFactory ObjectFactory { get; set; }

		public Task<IRepository<TInterface>> GetAsync<TInterface>() where TInterface : IEntity
		{
			IRepository<TInterface> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			returnValue = this.ObjectFactory.GetInstance<IRepository<TInterface>>();

			return Task.FromResult(returnValue);
		}

		public async Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>() where TInterface : IEntity
		{
			IReadOnlyRepository<TInterface> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IRepository repository = await this.GetAsync<TInterface>();

			if (repository is IReadOnlyRepository<TInterface>)
			{
				// ***
				// *** Cast the repository to IRepositry<T> and return it.
				// ***
				returnValue = repository as IReadOnlyRepository<TInterface>;

				if (returnValue == null)
				{
					throw new RepositoryNotReadableException(repository.GetType());
				}
			}
			else
			{
				throw new RepositoryNotDefinedException(typeof(TInterface));
			}

			return returnValue;
		}

		public async Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>() where TInterface : IEntity
		{
			IWritableRepository<TInterface> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IRepository repository = await this.GetAsync<TInterface>();

			if (repository is IWritableRepository<TInterface>)
			{
				// ***
				// *** Cast the repository to IRepositry<T> and return it.
				// ***
				returnValue = repository as IWritableRepository<TInterface>;

				if (returnValue == null)
				{
					throw new RepositoryNotWritableException(repository.GetType());
				}
			}
			else
			{
				throw new RepositoryNotDefinedException(typeof(TInterface));
			}

			return returnValue;
		}

		public async Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>() where TInterface : IEntity
		{
			IQueryableRepository<TInterface> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IRepository repository = await this.GetAsync<TInterface>();

			if (repository is IQueryableRepository<TInterface>)
			{
				// ***
				// *** Cast the repository to IRepositry<T> and return it.
				// ***
				returnValue = repository as IQueryableRepository<TInterface>;
			}
			else
			{
				throw new RepositoryNotDefinedException(typeof(TInterface));
			}

			return returnValue;
		}
	}
}

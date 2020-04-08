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
			return this.GetAsync<TInterface>(null);
		}

		public Task<IRepository<TInterface>> GetAsync<TInterface>(string name) where TInterface : IEntity
		{
			IRepository<TInterface> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			if (name == null)
			{
				returnValue = this.ObjectFactory.GetInstance<IRepository<TInterface>>();
			}
			else
			{
				returnValue = this.ObjectFactory.GetInstance<IRepository<TInterface>>(name);
			}

			return Task.FromResult(returnValue);
		}

		public Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>() where TInterface : IEntity
		{
			return this.GetReadOnlyAsync<TInterface>(null);
		}

		public async Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>(string name) where TInterface : IEntity
		{
			IReadOnlyRepository<TInterface> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IRepository repository = await this.GetAsync<TInterface>(name);

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

		public Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>() where TInterface : IEntity
		{
			return this.GetWritableAsync<TInterface>(null);
		}

		public async Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>(string name) where TInterface : IEntity
		{
			IWritableRepository<TInterface> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IRepository repository = await this.GetAsync<TInterface>(name);

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

		public Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>() where TInterface : IEntity
		{
			return this.GetQueryableAsync<TInterface>(null);
		}

		public async Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>(string name) where TInterface : IEntity
		{
			IQueryableRepository<TInterface> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IRepository repository = await this.GetAsync<TInterface>(name);

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

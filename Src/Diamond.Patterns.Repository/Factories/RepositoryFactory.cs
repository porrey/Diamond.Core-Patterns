// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
	public class RepositoryFactory : IRepositoryFactory
	{
		public RepositoryFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		public RepositoryFactory(IObjectFactory objectFactory, ILoggerSubscriber loggerSubscriber)
		{
			this.ObjectFactory = objectFactory;
			this.LoggerSubscriber = loggerSubscriber;
		}

		protected IObjectFactory ObjectFactory { get; set; }
		public ILoggerSubscriber LoggerSubscriber { get; set; } = new NullLoggerSubscriber();

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
				this.LoggerSubscriber.Verbose($"Retrieving IRepository for type '{typeof(TInterface)}'.");
				returnValue = this.ObjectFactory.GetInstance<IRepository<TInterface>>();
				this.LoggerSubscriber.AddToInstance(returnValue);
			}
			else
			{
				this.LoggerSubscriber.Verbose($"Retrieving IRepository for type '{typeof(TInterface)}' and container registration name '{name}'.");
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

			this.LoggerSubscriber.Verbose($"Retrieving IReadOnlyRepository for type '{typeof(TInterface)}' and container registration name '{name}'.");

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IRepository repository = await this.GetAsync<TInterface>(name);

			if (repository is IReadOnlyRepository<TInterface> castedRepository)
			{
				this.LoggerSubscriber.Verbose($"IRepository for type '{typeof(TInterface)}' and container registration name '{name}' was found.");

				// ***
				// *** Cast the repository to IRepositry<T> and return it.
				// ***
				returnValue = castedRepository;
				this.LoggerSubscriber.Verbose($"The repository '{repository.GetType().Name}' implements IReadOnlyRepository.");
			}
			else
			{
				this.LoggerSubscriber.Error($"The repository '{repository.GetType().Name}' does NOT implement IReadOnlyRepository. Throwing exception...");
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

			this.LoggerSubscriber.Verbose($"Retrieving IWritableRepository for type '{typeof(TInterface)}' and container registration name '{name}'.");

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IRepository repository = await this.GetAsync<TInterface>(name);

			if (repository is IWritableRepository<TInterface> castedRepository)
			{
				this.LoggerSubscriber.Verbose($"IRepository for type '{typeof(TInterface)}' and container registration name '{name}' was found.");

				// ***
				// *** Cast the repository to IRepositry<T> and return it.
				// ***
				returnValue = castedRepository;
				this.LoggerSubscriber.Verbose($"The repository '{repository.GetType().Name}' implements IWritableRepository.");
			}
			else
			{
				this.LoggerSubscriber.Error($"The repository '{repository.GetType().Name}' does NOT implement IWritableRepository. Throwing exception...");
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

			this.LoggerSubscriber.Verbose($"Retrieving IQueryableRepository for type '{typeof(TInterface)}' and container registration name '{name}'.");

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IRepository repository = await this.GetAsync<TInterface>(name);

			if (repository is IQueryableRepository<TInterface> castedRepository)
			{
				// ***
				// *** Cast the repository to IRepositry<T> and return it.
				// ***
				returnValue = castedRepository;
				this.LoggerSubscriber.Verbose($"The repository '{repository.GetType().Name}' implements IQueryableRepository.");
			}
			else
			{
				this.LoggerSubscriber.Error($"The repository '{repository.GetType().Name}' does NOT implement IQueryableRepository. Throwing exception...");
				throw new RepositoryNotDefinedException(typeof(TInterface));
			}

			return returnValue;
		}
	}
}

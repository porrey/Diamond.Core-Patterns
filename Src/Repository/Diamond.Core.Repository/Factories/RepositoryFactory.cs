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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Diamond.Core.Repository
{
	/// <summary>
	/// 
	/// </summary>
	public class RepositoryFactory : IRepositoryFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		public RepositoryFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="logger"></param>
		public RepositoryFactory(IServiceProvider serviceProvider, ILogger<RepositoryFactory> logger)
		{
			this.ServiceProvider = serviceProvider;
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ILogger<RepositoryFactory> Logger { get; set; } = new NullLogger<RepositoryFactory>();

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <returns></returns>
		public Task<IRepository<TInterface>> GetAsync<TInterface>() where TInterface : IEntity
		{
			return this.GetAsync<TInterface>(null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public Task<IRepository<TInterface>> GetAsync<TInterface>(string name) where TInterface : IEntity
		{
			IRepository<TInterface> returnValue = null;

			//
			// Find the repository that supports the given type.
			//
			if (name == null)
			{
				this.Logger.LogDebug($"Retrieving IRepository for type '{typeof(TInterface)}'.");
				returnValue = this.ServiceProvider.GetService<IRepository<TInterface>>();

				if (returnValue == null)
				{
					throw new RepositoryNotDefinedException(typeof(TInterface));
				}
			}
			else
			{
				this.Logger.LogDebug($"Retrieving IRepository for type '{typeof(TInterface)}' and container registration name '{name}'.");
				IEnumerable<IRepository<TInterface>> repositories = this.ServiceProvider.GetRequiredService<IEnumerable<IRepository<TInterface>>>();
				returnValue = repositories.Where(t => t.Name == name).SingleOrDefault();

				if (returnValue == null)
				{
					this.Logger.LogWarning($"A IRepository for type '{typeof(TInterface)}' and container registration name '{name}'.");
					throw new RepositoryNotDefinedException(typeof(TInterface), name);
				}
			}

			return Task.FromResult(returnValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <returns></returns>
		public Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>() where TInterface : IEntity
		{
			return this.GetReadOnlyAsync<TInterface>(null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public async Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>(string name) where TInterface : IEntity
		{
			IReadOnlyRepository<TInterface> returnValue = null;

			this.Logger.LogDebug($"Retrieving IReadOnlyRepository for type '{typeof(TInterface)}' and container registration name '{name}'.");

			//
			// Find the repository that supports the given type.
			//
			IRepository repository = await this.GetAsync<TInterface>(name);

			if (repository is IReadOnlyRepository<TInterface> castedRepository)
			{
				this.Logger.LogDebug($"IRepository for type '{typeof(TInterface)}' and container registration name '{name}' was found.");

				//
				// Cast the repository to IRepositry<T> and return it.
				//
				returnValue = castedRepository;
				this.Logger.LogDebug($"The repository '{repository.GetType().Name}' implements IReadOnlyRepository.");
			}
			else
			{
				this.Logger.LogError($"The repository '{repository.GetType().Name}' does NOT implement IReadOnlyRepository. Throwing exception...");
				throw new RepositoryNotReadableException(typeof(TInterface));
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <returns></returns>
		public Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>() where TInterface : IEntity
		{
			return this.GetWritableAsync<TInterface>(null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public async Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>(string name) where TInterface : IEntity
		{
			IWritableRepository<TInterface> returnValue = null;

			this.Logger.LogDebug($"Retrieving IWritableRepository for type '{typeof(TInterface)}' and container registration name '{name}'.");

			//
			// Find the repository that supports the given type.
			//
			IRepository repository = await this.GetAsync<TInterface>(name);

			if (repository is IWritableRepository<TInterface> castedRepository)
			{
				this.Logger.LogDebug($"IRepository for type '{typeof(TInterface)}' and container registration name '{name}' was found.");

				//
				// Cast the repository to IRepositry<T> and return it.
				//
				returnValue = castedRepository;
				this.Logger.LogDebug($"The repository '{repository.GetType().Name}' implements IWritableRepository.");
			}
			else
			{
				this.Logger.LogError($"The repository '{repository.GetType().Name}' does NOT implement IWritableRepository. Throwing exception...");
				throw new RepositoryNotWritableException(typeof(TInterface));
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <returns></returns>
		public Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>() where TInterface : IEntity
		{
			return this.GetQueryableAsync<TInterface>(null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public async Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>(string name) where TInterface : IEntity
		{
			IQueryableRepository<TInterface> returnValue = null;

			this.Logger.LogDebug($"Retrieving IQueryableRepository for type '{typeof(TInterface)}' and container registration name '{name}'.");

			//
			// Find the repository that supports the given type.
			//
			IRepository repository = await this.GetAsync<TInterface>(name);

			if (repository is IQueryableRepository<TInterface> castedRepository)
			{
				//
				// Cast the repository to IRepositry<T> and return it.
				//
				returnValue = castedRepository;
				this.Logger.LogDebug($"The repository '{repository.GetType().Name}' implements IQueryableRepository.");
			}
			else
			{
				this.Logger.LogError($"The repository '{repository.GetType().Name}' does NOT implement IQueryableRepository. Throwing exception...");
				throw new RepositoryNotQueryableException(typeof(TInterface));
			}

			return returnValue;
		}
	}
}

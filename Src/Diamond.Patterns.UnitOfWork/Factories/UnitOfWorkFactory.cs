using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.UnitOfWork
{
	/// <summary>
	/// This is a generic repository factory that can return a repository
	/// for any given entity interface.
	/// </summary>
	public class UnitOfWorkFactory : IUnitOfWorkFactory
	{
		public UnitOfWorkFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		protected IObjectFactory ObjectFactory { get; set; }

		public Task<IUnitOfWork<TResult, TSourceItem>> GetAsync<TResult, TSourceItem>(string name)
		{
			IUnitOfWork<TResult, TSourceItem> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(IUnitOfWork<TResult, TSourceItem>);

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IEnumerable<IUnitOfWork> items = this.ObjectFactory.GetAllInstances<IUnitOfWork>();
			IEnumerable<IUnitOfWork> keyItems = items.Where(t => t.Key == name);

			// ***
			// *** Within the list, find the target decorator.
			// ***
			foreach (IUnitOfWork item in keyItems)
			{
				if (targetType.IsInstanceOfType(item))
				{
					returnValue = (IUnitOfWork<TResult, TSourceItem>)item;
					break;
				}
			}

			// ***
			// *** Check the result.
			// ***
			if (returnValue == null)
			{
				throw new UnitOfWorkNotFoundException<TResult, TSourceItem>(name);
			}

			return Task.FromResult(returnValue);
		}

		public async Task<IUnitOfWork<TResult, TSourceItem>> GetAsync<TResult, TSourceItem>()
		{
			return await this.GetAsync<TResult, TSourceItem>(null);
		}
	}
}

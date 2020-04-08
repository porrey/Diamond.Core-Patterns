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

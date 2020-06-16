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
using Diamond.Patterns.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diamond.Patterns.Rules
{
	/// <summary>
	/// Defines a generic repository factory that can be used to retrieve
	/// an object that implements <see cref="IRule<TItem, TResult>" from the container.
	/// </summary>
	public class RulesFactory : IRulesFactory
	{
		/// <summary>
		/// Creates an instance of <see cref="IRule<TItem, TResult>" with the
		/// specififed instance of <see cref="IObjectFactory">."</see>
		/// </summary>
		/// <param name="objectFactory"></param>
		public RulesFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		/// <summary>
		/// Gets/sets the internal instance of <see cref="IObjectFactory">.
		/// </summary>
		protected IObjectFactory ObjectFactory { get; set; }

		/// <summary>
		/// Get all model rule instances registered based on TInterface.
		/// </summary>
		/// <typeparam name="TItem">The type of the model being validated.</typeparam>
		/// <returns>A list of <see cref="IRule<TItem, TResult>" instances.</returns>
		public Task<IEnumerable<IRule<TItem>>> GetAllAsync<TItem>()
		{
			return this.GetAllAsync<TItem>(null);
		}

		/// <summary>
		/// Get all model rule instances registered based on TInterface and group name.
		/// </summary>
		/// <typeparam name="TItem">The type of the model being validated.</typeparam>
		/// <returns>A list of <see cref="IRule<TItem, TResult>" instances.</returns>
		public Task<IEnumerable<IRule<TItem>>> GetAllAsync<TItem>(string group)
		{
			IList<IRule<TItem>> returnValue = new List<IRule<TItem>>();

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(IRule<TItem>);

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IEnumerable<IRule> items = this.ObjectFactory.GetAllInstances<IRule>();

			if (!String.IsNullOrEmpty(group))
			{
				items = items.Where(t => t.Group == group);
			}

			if (items.Count() > 0)
			{
				foreach (IRule item in items)
				{
					if (targetType.IsInstanceOfType(item))
					{
						returnValue.Add((IRule<TItem>)item);
					}
				}
			}
			else
			{
				if (!String.IsNullOrWhiteSpace(group))
				{
					throw new RulesNotFoundException<TItem>(group);
				}
				else
				{
					throw new RulesNotFoundException<TItem>();
				}
			}

			return Task.FromResult<IEnumerable<IRule<TItem>>>(returnValue);
		}

		/// <summary>
		/// Get all model rule instances registered based on TInterface and group name.
		/// </summary>
		/// <typeparam name="TItem">The type of the model being validated.</typeparam>
		/// <returns>A list of <see cref="IRule<TItem, TResult>" instances.</returns>
		public Task<IEnumerable<IRule<TItem, TResult>>> GetAllAsync<TItem, TResult>()
		{
			return this.GetAllAsync<TItem, TResult>(null);
		}

		/// <summary>
		/// Get all model rule instances registered based on TInterface and group name.
		/// </summary>
		/// <typeparam name="TItem">The type of the model being validated.</typeparam>
		/// <returns>A list of <see cref="IRule<TItem, TResult>" instances.</returns>
		public Task<IEnumerable<IRule<TItem, TResult>>> GetAllAsync<TItem, TResult>(string group)
		{
			IList<IRule<TItem, TResult>> returnValue = new List<IRule<TItem, TResult>>();

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(IRule<TItem, TResult>);

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IEnumerable<IRule> items = this.ObjectFactory.GetAllInstances<IRule>();

			if (!String.IsNullOrEmpty(group))
			{
				items = items.Where(t => t.Group == group);
			}

			if (items.Count() > 0)
			{
				foreach (IRule item in items)
				{
					if (targetType.IsInstanceOfType(item))
					{
						returnValue.Add((IRule<TItem, TResult>)item);
					}
				}
			}
			else
			{
				if (!String.IsNullOrWhiteSpace(group))
				{
					throw new RulesNotFoundException<TItem>(group);
				}
				else
				{
					throw new RulesNotFoundException<TItem>();
				}
			}

			return Task.FromResult<IEnumerable<IRule<TItem, TResult>>>(returnValue);
		}
	}
}

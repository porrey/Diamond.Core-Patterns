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
using System;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Specification
{
	public class SpecificationFactory : ISpecificationFactory
	{
		public SpecificationFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		public SpecificationFactory(IObjectFactory objectFactory, ILoggerSubscriber loggerSubscriber)
		{
			this.ObjectFactory = objectFactory;
			this.LoggerSubscriber = loggerSubscriber;
		}

		public ILoggerSubscriber LoggerSubscriber { get; set; } = new NullLoggerSubscriber();
		protected IObjectFactory ObjectFactory { get; set; }

		public async Task<ISpecification<TResult>> GetAsync<TResult>()
		{
			return await this.GetAsync<TResult>(null);
		}

		public Task<ISpecification<TResult>> GetAsync<TResult>(string name)
		{
			ISpecification<TResult> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(ISpecification<TResult>);
			this.LoggerSubscriber.Verbose($"Finding a Specification with container registration name '{name}' and Target Type '{targetType.Name}'.");

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			ISpecification item = this.ObjectFactory.GetInstance<ISpecification>(name);

			// ***
			// *** Within the list, find the target decorator.
			// ***
			if (item != null)
			{
				if (targetType.IsInstanceOfType(item))
				{
					this.LoggerSubscriber.Verbose($"The Specification '{name}' and Target Type '{targetType.Name}' was found.");
					returnValue = (ISpecification<TResult>)item;
					this.LoggerSubscriber.AddToInstance(returnValue);
				}
				else
				{
					this.LoggerSubscriber.Verbose($"The Specification key '{name}' and Target Type '{targetType.Name}' was NOT found. Throwing exception...");
					throw new SpecificationNotFoundException<TResult>(name);
				}
			}

			return Task.FromResult(returnValue);
		}

		public Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>(string name)
		{
			ISpecification<TParameter, TResult> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(ISpecification<TParameter, TResult>);
			this.LoggerSubscriber.Verbose($"Finding a Specification with container registration name '{name}' and Target Type '{targetType.Name}'.");

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			ISpecification item = this.ObjectFactory.GetInstance<ISpecification>(name);

			// ***
			// *** Within the list, find the target decorator.
			// ***
			if (item != null)
			{
				if (targetType.IsInstanceOfType(item))
				{
					this.LoggerSubscriber.Verbose($"The Specification '{name}' and Target Type '{targetType.Name}' was found.");
					returnValue = (ISpecification<TParameter, TResult>)item;
					this.LoggerSubscriber.AddToInstance(returnValue);
				}
				else
				{
					this.LoggerSubscriber.Verbose($"The Specification key '{name}' and Target Type '{targetType.Name}' was NOT found. Throwing exception...");
					throw new SpecificationNotFoundException<TParameter, TResult>(name);
				}
			}

			return Task.FromResult(returnValue);
		}

		public async Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>()
		{
			return await this.GetAsync<TParameter, TResult>(null);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Decorator
{
	/// <summary>
	/// Defines a generic repository factory that can be used to retrieve
	/// an object that implements IDecorator<TItem, TResult> from the container.
	/// </summary>
	public class DecoratorFactory : IDecoratorFactory
	{
		public DecoratorFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		protected IObjectFactory ObjectFactory { get; set; }

		public Task<IDecorator<TItem, TResult>> GetAsync<TItem, TResult>()
		{
			IDecorator<TItem, TResult> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(IDecorator<TItem, TResult>);

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IEnumerable<IDecorator> items = this.ObjectFactory.GetAllInstances<IDecorator>();

			// ***
			// *** Within the list, find the target decorator.
			// ***
			foreach (IDecorator item in items)
			{
				if (targetType.IsInstanceOfType(item))
				{
					returnValue = (IDecorator<TItem, TResult>)item;
					break;
				}
			}

			// ***
			// *** Check the result.
			// ***
			if (returnValue == null)
			{
				throw new DecoratorNotFoundException<TItem, TResult>();
			}

			return Task.FromResult(returnValue);
		}

		public Task<IDecorator<TItem, TResult>> GetAsync<TItem, TResult>(string name)
		{
			IDecorator<TItem, TResult> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(IDecorator<TItem, TResult>);

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IDecorator decorator = this.ObjectFactory.GetInstance<IDecorator>(name);

			// ***
			// *** Within the list, find the target decorator.
			// ***
			if (decorator != null)
			{
				if (targetType.IsInstanceOfType(decorator))
				{
					returnValue = (IDecorator<TItem, TResult>)decorator;
				}
				else
				{
					throw new DecoratorNotFoundException<TItem, TResult>(name);
				}
			}

			return Task.FromResult(returnValue);
		}
	}
}

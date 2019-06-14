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
			IEnumerable<IDecorator> decorators = this.ObjectFactory.GetAllInstances<IDecorator>();

			// ***
			// *** Within the list, find the target decorator.
			// ***
			foreach (IDecorator decorator in decorators)
			{
				if (targetType.IsInstanceOfType(decorator))
				{
					returnValue = (IDecorator<TItem, TResult>)decorator;
					break;
				}
				else
				{
					// ***
					// *** Call dispose on the unused instance.
					// ***
					TryDisposable<IDecorator<TItem, TResult>>.Dispose(decorator);
				}
			}

			// ***
			// *** Check the result.
			// ***
			if (returnValue == null)
			{
				throw new Exception(String.Format("A decorator of type '{0}' has not been configured.", targetType.Name));
			}

			return Task.FromResult(returnValue);
		}
	}
}

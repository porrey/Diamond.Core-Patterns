using System;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Specification
{
	internal class SpecificationFactory : ISpecificationFactory
	{
		public SpecificationFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

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
					returnValue = (ISpecification<TResult>)item;
				}
				else
				{
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
					returnValue = (ISpecification<TParameter, TResult>)item;
				}
				else
				{
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

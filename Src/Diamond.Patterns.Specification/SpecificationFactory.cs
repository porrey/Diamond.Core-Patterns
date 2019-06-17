using System.Linq;
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
			return (await this.ObjectFactory.ResolveByInterfaceAsync<ISpecification<TResult>>()).FirstOrDefault();
		}
	}
}

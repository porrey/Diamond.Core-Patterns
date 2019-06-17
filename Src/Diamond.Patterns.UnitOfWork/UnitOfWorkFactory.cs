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

		public async Task<IUnitOfWork<TSourceItem>> GetAsync<TSourceItem>()
		{
			return (await this.ObjectFactory.ResolveByInterfaceAsync<IUnitOfWork<TSourceItem>>()).FirstOrDefault();
		}
	}
}

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

		public Task<IUnitOfWork<TSourceItem>> GetAsync<TSourceItem>()
		{
			IUnitOfWork<TSourceItem> returnValue = null;

			// ***
			// *** Find the repository that supports the given type.
			// ***
			returnValue = this.ObjectFactory.GetInstance<IUnitOfWork<TSourceItem>>();

			return Task.FromResult(returnValue);
		}
	}
}

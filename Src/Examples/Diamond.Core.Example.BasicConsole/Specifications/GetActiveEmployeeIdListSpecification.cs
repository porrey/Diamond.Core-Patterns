using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Diamond.Core.Specification;

namespace Diamond.Core.Example.BasicConsole
{
	public class GetActiveEmployeeIdListSpecification : ISpecification<IEnumerable<int>>
	{
		public GetActiveEmployeeIdListSpecification(IRepositoryFactory repositoryFactory)
		{
			this.RepositoryFactory = repositoryFactory;
		}

		public string Name => "GetActiveEmployeeIdList";
		protected IRepositoryFactory RepositoryFactory { get; set; }

		public async Task<IEnumerable<int>> ExecuteSelectionAsync()
		{
			IEnumerable<int> returnValue = Array.Empty<int>();

			//
			// Get a repository for IEmployeeEntity.
			//
			IReadOnlyRepository<IEmployeeEntity> repository = await this.RepositoryFactory.GetReadOnlyAsync<IEmployeeEntity>();

			//
			// Get all active employees ID's.
			//
			returnValue = (await repository.GetAsync(t => t.Active)).Select(t => t.Id);

			//
			// Since we are using transient lifetimes, we need to dispose.
			//
			await repository.TryDisposeAsync();

			return returnValue;
		}
	}
}

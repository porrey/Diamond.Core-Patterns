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
			IReadOnlyRepository<IEmployeeEntity> repostory = await this.RepositoryFactory.GetReadOnlyAsync<IEmployeeEntity>();

			//
			// Get all active employees ID's.
			//
			returnValue = (await repostory.GetAsync(t => t.Active)).Select(t => t.Id);

			return returnValue;
		}
	}
}

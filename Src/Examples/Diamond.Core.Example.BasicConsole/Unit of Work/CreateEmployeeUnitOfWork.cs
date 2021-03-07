using System.Threading.Tasks;
using Diamond.Core.Repository;
using Diamond.Core.UnitOfWork;

namespace Diamond.Core.Example.BasicConsole
{
	public class CreateEmployeeUnitOfWork : IUnitOfWork<(bool, int?), IEmployeeEntity>
	{
		public CreateEmployeeUnitOfWork(IRepositoryFactory repositoryFactory)
		{
			this.RepositoryFactory = repositoryFactory;
		}

		public string Name => "CreateEmployee";
		protected IRepositoryFactory RepositoryFactory { get; set; }

		public async Task<(bool, int?)> CommitAsync(IEmployeeEntity item)
		{
			(bool result, int? employeeId) = (false, null);

			//
			// Get a repository for IEmployeeEntity.
			//
			IWritableRepository<IEmployeeEntity> repository = await this.RepositoryFactory.GetWritableAsync<IEmployeeEntity>();

			//
			// Add the item to the database.
			//
			(bool addResult, IEmployeeEntity newEntity) = await repository.AddAsync(item);

			//
			// Check the result.
			//
			if (addResult)
			{
				//
				// The entity was successfully added to the database.
				//
				result = true;
				employeeId = newEntity.Id;
			}
			else
			{
				//
				// The entity failed to be added.
				//
				result = true;
				employeeId = null;
			}

			return (result, employeeId);
		}
	}
}

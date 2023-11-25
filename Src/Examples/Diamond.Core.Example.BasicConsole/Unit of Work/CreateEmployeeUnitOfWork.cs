using System;
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Diamond.Core.UnitOfWork;

namespace Diamond.Core.Example.BasicConsole
{
	public class CreateEmployeeUnitOfWork : DisposableObject, IUnitOfWork<(bool, int?), IEmployeeEntity>
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
			(int affected, IEmployeeEntity newEntity) = await repository.AddAsync(item);

			//
			// Check the result.
			//
			if (affected > 0)
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

			//
			// Since we are using transient lifetimes, we need to dispose.
			//
			await repository.TryDisposeAsync();

			return (result, employeeId);
		}
	}
}

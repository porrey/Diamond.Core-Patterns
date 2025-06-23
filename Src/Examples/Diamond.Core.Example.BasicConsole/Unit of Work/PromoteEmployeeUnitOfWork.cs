using System;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Diamond.Core.UnitOfWork;

namespace Diamond.Core.Example.BasicConsole
{
	public class PromoteEmployeeUnitOfWork : DisposableObject, IUnitOfWork<(bool, IEmployeeEntity), (int EmployeeId, string JobTitle, decimal PercentRaise)>
	{
		public PromoteEmployeeUnitOfWork(IRepositoryFactory repositoryFactory)
		{
			this.RepositoryFactory = repositoryFactory;
		}

		public string Name => "PromoteEmployee";
		protected IRepositoryFactory RepositoryFactory { get; set; }

		public async Task<(bool, IEmployeeEntity)> CommitAsync((int EmployeeId, string JobTitle, decimal PercentRaise) item)
		{
			(bool returnValue, IEmployeeEntity updatedEmployee) = (false, null);

			//
			// Get a repository for IEmployeeEntity.
			//
			using IWritableRepository<IEmployeeEntity> repository = await this.RepositoryFactory.GetWritableAsync<IEmployeeEntity>();

			//
			// Get the employee.
			//
			IEmployeeEntity employee = (await repository.AsReadOnly().GetAsync(t => t.Id == item.EmployeeId)).SingleOrDefault();

			//
			// Check the result.
			//
			if (employee != null)
			{
				//
				// Update the employee's compensation.
				//
				employee.JobTitle = item.JobTitle;
				employee.Compensation += (employee.Compensation * item.PercentRaise);

				//
				// Update the database.
				//
				int affected = await repository.UpdateAsync(employee);

				if (affected > 0)
				{
					updatedEmployee = employee;
					returnValue = true;
				}
				else
				{
					returnValue = false;
				}
			}
			else
			{
				//
				// Employee not found
				//
				throw new Exception($"An employee with ID {item.EmployeeId} does not exist.");
			}

			//
			// Since we are using transient lifetimes, we need to dispose.
			//
			await repository.TryDisposeAsync();

			return (returnValue, updatedEmployee);
		}
	}
}

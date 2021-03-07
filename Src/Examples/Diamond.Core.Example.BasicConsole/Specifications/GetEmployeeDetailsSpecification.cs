﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Diamond.Core.Specification;

namespace Diamond.Core.Example.BasicConsole
{
	public class GetEmployeeDetailsSpecification : ISpecification<int, IEmployeeEntity>
	{
		public GetEmployeeDetailsSpecification(IRepositoryFactory repositoryFactory)
		{
			this.RepositoryFactory = repositoryFactory;
		}

		public string Name => "GetEmployeeDetails";
		protected IRepositoryFactory RepositoryFactory { get; set; }

		public async Task<IEmployeeEntity> ExecuteSelectionAsync(int employeeId)
		{
			IEmployeeEntity returnValue = null;

			//
			// Get a repository for IEmployeeEntity.
			//
			IReadOnlyRepository<IEmployeeEntity> repostory = await this.RepositoryFactory.GetReadOnlyAsync<IEmployeeEntity>();

			//
			// Get all active employees ID's.
			//
			returnValue = (await repostory.GetAsync(t => t.Id == employeeId)).SingleOrDefault();

			return returnValue;
		}
	}
}

//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.BasicConsole
{
	/// <summary>
	/// This step will delete the current database and recreate it.
	/// </summary>
	public class SampleWorkStep1 : WorkflowItem
	{
		public SampleWorkStep1(ILogger<SampleWorkStep1> logger, IRepositoryFactory repositoryFactory)
			: base(logger)
		{
			this.RepositoryFactory = repositoryFactory;
		}

		public override string Name => $"Sample Step {this.Ordinal}";
		public override string Group => WellKnown.Workflow.SampleWorkflow;
		public override int Ordinal => 1;
		protected IRepositoryFactory RepositoryFactory { get; set; }

		protected override async Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Get a repository for IEmployeeEntity.
			//
			IQueryableRepository<IEmployeeEntity> repository = await this.RepositoryFactory.GetQueryableAsync<IEmployeeEntity>();

			//
			// Get the context.
			//
			IRepositoryContext db = await repository.GetContextAsync();

			//
			// Delete the database
			//
			this.Logger.LogInformation("Deleting existing database.");
			await db.EnsureDeletedAsync();

			//
			// Create the database
			//
			this.Logger.LogInformation($"Creating new empty database.");
			if (await db.EnsureCreatedAsync())
			{
				returnValue = true;
				this.Logger.LogInformation("Database was successfully created.");
			}
			else
			{
				await this.StepFailedAsync(context, "Failed to create new database.");
			}

			return returnValue;
		}
	}
}

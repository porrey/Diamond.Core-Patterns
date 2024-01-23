//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
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
using System;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.Extensions.DependencyInjection;
using Diamond.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class CreateCommandDirect : CreateCommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public CreateCommandDirect(ILogger<CreateCommandDirect> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
			: base(logger)
		{
			this.RepositoryFactory = repositoryFactory;
			this.Mapper = mapper;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected IMapper Mapper { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override async Task<int> OnHandleCommand(Invoice item)
		{
			int returnValue = 0;

			//
			// Get a writable repository for IInvoice.
			//
			this.Logger.LogDebug("Retrieving a writable repository for IInvoice.");
			IWritableRepository<IInvoice> repository = await this.RepositoryFactory.GetWritableAsync<IInvoice>();

			//
			// Ensure the database exists.
			//
			IRepositoryContext context = await repository.AsQueryable().GetContextAsync();
			await context.EnsureDeletedAsync();
			await context.EnsureCreatedAsync();

			//
			// Create a new entity.
			//
			IInvoice model = await repository.ModelFactory.CreateAsync();

			//
			// Set the properties.
			//
			this.Mapper.Map(item, model);

			try
			{
				//
				// Attempt to create the item.
				//
				(int affected, IInvoice newItem) = await repository.AddAsync(model);

				if (affected > 0)
				{
					this.Logger.LogInformation("An invoice with ID = {id} has been created.", newItem.Id);
					returnValue = 0;
				}
				else
				{
					this.Logger.LogError("The invoice could not be created.");
					returnValue = 1;
				}
			}
			catch (DbUpdateException dbex)
			{
				if (dbex.InnerException != null && dbex.InnerException.Message.Contains("duplicate"))
				{
					this.Logger.LogError("An invoice with number '{number}' already exists.", item.Number);
				}
				else
				{
					this.Logger.LogError(dbex, "Exception while creating invoice.");
				}
			}
			catch (Exception ex)
			{
				this.Logger.LogError(ex, "Exception while creating invoice.");
			}

			return returnValue;
		}
	}
}

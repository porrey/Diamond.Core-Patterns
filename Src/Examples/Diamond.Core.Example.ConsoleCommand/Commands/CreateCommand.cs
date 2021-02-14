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
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class CreateCommand : Command
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public CreateCommand(ILogger<CreateCommand> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
			: base("create", "Creates a new invoice item.")
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
			this.Mapper = mapper;

			this.AddOption(new Option<string>($"--{nameof(Invoice.Number).ToLower()}", "Invoice Number.")
			{
				IsRequired = true
			});

			this.AddOption(new Option<string>($"--{nameof(Invoice.Description).ToLower()}", "Invoice Description.")
			{
				IsRequired = true
			});

			this.AddOption(new Option<float>($"--{nameof(Invoice.Total).ToLower()}", "Invoice Total.")
			{
				IsRequired = true
			});

			this.AddOption(new Option<bool>($"--{nameof(Invoice.Paid).ToLower()}", "Indicates if the invoice has been paid or not.")
			{
				IsRequired = false
			});

			this.Handler = CommandHandler.Create<Invoice>(async (p) =>
			{
				return await this.OnHandleCommand(p);
			});
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<CreateCommand> Logger { get; set; } = new NullLogger<CreateCommand>();

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
		protected async Task<int> OnHandleCommand(Invoice item)
		{
			int returnValue = 0;

			//
			// Get a writable repository for IInvoice.
			//
			this.Logger.LogDebug("Retrieving a writable repository for IInvoice.");
			IWritableRepository<IInvoice> repository = await this.RepositoryFactory.GetWritableAsync<IInvoice>();

			//var context = await repository.GetContextAsync();
			//await context.EnsureDeleted();
			//await context.EnsureCreated();

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
				(bool result, IInvoice newItem) = await repository.AddAsync(model);

				if (result)
				{
					this.Logger.LogInformation($"An invoice with ID = {newItem.Id} has been created.");
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
					this.Logger.LogError($"An invoice with number '{item.Number}' already exists.");
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

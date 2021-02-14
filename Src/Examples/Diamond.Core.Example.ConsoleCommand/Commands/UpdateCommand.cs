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
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdateCommand : Command
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public UpdateCommand(ILogger<UpdateCommand> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
			: base("update", "Updates an invoice.")
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
		protected ILogger<UpdateCommand> Logger { get; set; } = new NullLogger<UpdateCommand>();

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

			//
			// Get the invoice.
			//
			IInvoice exisingItem = (await repository.GetAsync(t => t.Number == item.Number)).SingleOrDefault();

			if (exisingItem != null)
			{
				//
				// Apply the patch 
				//
				this.Mapper.Map(item, exisingItem);

				try
				{
					//
					// Update the data.
					//
					bool result = await repository.UpdateAsync(exisingItem);

					if (result)
					{
						this.Logger.LogInformation($"Successfully update invoice {item.Number} [ID = {exisingItem.Id}].");
						returnValue = 0;
					}
					else
					{
						this.Logger.LogError($"The invoice with invoice number '{item.Number}' could not be updated.");
					}
				}
				catch (Exception ex)
				{
					this.Logger.LogError(ex, "Exception while updating invoice.");
				}
			}
			else
			{
				this.Logger.LogInformation($"An invoice with invoice number '{item.Number}' could not be found.");
			}

			return returnValue;
		}
	}
}

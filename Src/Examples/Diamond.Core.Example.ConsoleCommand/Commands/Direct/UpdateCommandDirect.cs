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
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdateCommandDirect : UpdateCommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public UpdateCommandDirect(ILogger<UpdateCommandDirect> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
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
			// Get the invoice.
			//
			IInvoice exisingItem = (await repository.AsReadOnly().GetAsync(t => t.Number == item.Number)).SingleOrDefault();

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
					int affected = await repository.UpdateAsync(exisingItem);

					if (affected > 0)
					{
						this.Logger.LogInformation("Successfully update invoice {number} [ID = {id}].", item.Number, exisingItem.Id);
						returnValue = 0;
					}
					else
					{
						this.Logger.LogError("The invoice with invoice number '{number}' could not be updated.", item.Number);
					}
				}
				catch (Exception ex)
				{
					this.Logger.LogError(ex, "Exception while updating invoice.");
				}
			}
			else
			{
				this.Logger.LogInformation("An invoice with invoice number '{number}' could not be found.", item.Number);
			}

			return returnValue;
		}
	}
}

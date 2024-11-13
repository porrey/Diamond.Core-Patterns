//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class DeleteCommandDirect : DeleteCommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		public DeleteCommandDirect(ILogger<DeleteCommandDirect> logger, IRepositoryFactory repositoryFactory)
			: base(logger)
		{
			this.RepositoryFactory = repositoryFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoice"></param>
		/// <returns></returns>
		protected override async Task<int> OnHandleCommand(InvoiceNumber invoice)
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
			IInvoice exisingItem = (await repository.AsReadOnly().GetAsync(t => t.Number == invoice.Number)).SingleOrDefault();

			if (exisingItem != null)
			{
				//
				// Delete the data.
				//
				int affected = await repository.DeleteAsync(exisingItem);

				if (affected > 0)
				{
					this.Logger.LogInformation("Successfully delete invoice '{invoice}' [ID = {id}]", invoice.Number, exisingItem.Id);
					returnValue = 0;
				}
				else
				{
					this.Logger.LogError("The invoice with invoice number '{invoice}' could not be deleted [ID = {id}].", invoice.Number, exisingItem.Id);
				}
			}
			else
			{
				this.Logger.LogError("An invoice with invoice number '{invoice}' could not be found.", invoice.Number);
			}

			return returnValue;
		}
	}
}

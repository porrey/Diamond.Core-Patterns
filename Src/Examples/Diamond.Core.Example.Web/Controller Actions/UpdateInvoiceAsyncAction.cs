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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.AspNetCore.DoAction;
using Diamond.Core.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdateInvoiceAsyncAction : DoActionTemplate<(string InvoiceNumber, JsonPatchDocument<Invoice> Invoice), Invoice>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public UpdateInvoiceAsyncAction(ILogger<UpdateInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
			: base(logger)
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
			this.Mapper = mapper;
		}

		/// <summary>
		/// Holds the reference to <see cref="IRepositoryFactory"/>.
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
		public override Task<(bool, string)> ValidateModel((string InvoiceNumber, JsonPatchDocument<Invoice> Invoice) item)
		{
			(bool result, string errorString) = (true, null);

			//
			// Get the model from the patch.
			//
			Invoice patched = new Invoice();
			item.Invoice.ApplyTo(patched);

			//
			// Check the invoice number.
			//
			if (patched.Number != null)
			{
				result = false;
				errorString = "Invoice Number cannot be updated.";
			}

			return Task.FromResult((result, errorString));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override async Task<IControllerActionResult<Invoice>> OnExecuteActionAsync((string InvoiceNumber, JsonPatchDocument<Invoice> Invoice) item)
		{
			ControllerActionResult<Invoice> returnValue = new ControllerActionResult<Invoice>();

			//
			// Get a writable repository for IInvoice.
			//
			this.Logger.LogDebug("Retrieving a writable repository for IInvoice.");
			IWritableRepository<IInvoice> repository = await this.RepositoryFactory.GetWritableAsync<IInvoice>();

			//
			// Get the invoice.
			//
			IInvoice exisingItem = (await repository.AsReadOnly().GetAsync(t => t.Number == item.InvoiceNumber)).SingleOrDefault();

			if (exisingItem != null)
			{
				//
				// Apply the patch 
				//
				Invoice patched = new Invoice();
				this.Mapper.Map(exisingItem, patched);
				item.Invoice.ApplyTo(patched);

				//
				// Update the existing item.
				//
				this.Mapper.Map(patched, exisingItem);

				//
				// Update the data.
				//
				bool result = await repository.UpdateAsync(exisingItem);

				if (result)
				{
					returnValue.ResultDetails = DoActionResult.Ok();
					returnValue.Result = this.Mapper.Map<Invoice>(exisingItem);
				}
				else
				{
					returnValue.ResultDetails = DoActionResult.BadRequest($"The invoice with invoice number '{item.InvoiceNumber}' could not be updated.");
				}
			}
			else
			{
				returnValue.ResultDetails = DoActionResult.NotFound($"An invoice with invoice number '{item.InvoiceNumber}' could not be found.");
			}

			return returnValue;
		}
	}
}

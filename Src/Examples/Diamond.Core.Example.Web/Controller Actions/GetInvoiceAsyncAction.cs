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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.AspNetCore.DoAction;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class GetInvoiceAsyncAction : DoActionTemplate<string, Invoice>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public GetInvoiceAsyncAction(ILogger<GetInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
			: base(logger)
		{
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
		/// <param name="invoiceNumber"></param>
		/// <returns></returns>
		protected override async Task<IControllerActionResult<Invoice>> OnExecuteActionAsync(string invoiceNumber)
		{
			ControllerActionResult<Invoice> returnValue = new ControllerActionResult<Invoice>();

			//
			// Get a read-only repository for IInvoice.
			//
			this.Logger.LogDebug("Retrieving a read-only repository for IInvoice.");
			using IReadOnlyRepository<IInvoice> repository = await this.RepositoryFactory.GetReadOnlyAsync<IInvoice>();

			//
			// Attempt to create the item.
			//
			IInvoice item = (await repository.GetAsync(t => t.Number == invoiceNumber)).SingleOrDefault();

			if (item != null)
			{
				returnValue.ResultDetails = DoActionResult.Ok();
				returnValue.Result = this.Mapper.Map<Invoice>(item);
			}
			else
			{
				IDictionary<string, object> extensions = new Dictionary<string, object>
				{
					{ "Number", invoiceNumber }
				};

				returnValue.ResultDetails = DoActionResult.NotFound($"An invoice with invoice number '{invoiceNumber}' could not be found.", null, null, extensions);
			}

			return returnValue;
		}
	}
}

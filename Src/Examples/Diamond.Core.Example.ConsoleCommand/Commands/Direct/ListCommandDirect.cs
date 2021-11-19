﻿//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.CommandLine.Model;
using Diamond.Core.Extensions.DependencyInjection;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class ListCommandDirect : ListCommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		public ListCommandDirect(ILogger<ListCommandDirect> logger, IRepositoryFactory repositoryFactory)
			: base(logger)
		{
			this.RepositoryFactory = repositoryFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		[Dependency(OverrideValue = false, Required = true)]
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override async Task<int> OnHandleCommand(NullModel item)
		{
			int returnValue = 0;

			//
			// Get a read-only repository for IInvoice.
			//
			this.Logger.LogDebug("Retrieving read-only repository for IInvoice.");
			IReadOnlyRepository<IInvoice> repository = await this.RepositoryFactory.GetReadOnlyAsync<IInvoice>();

			//
			// Query all of the items and create a InvoiceReponse for each.
			//
			this.Logger.LogDebug("Retrieving all Invoice items from data storage.");
			IEnumerable<IInvoice> items = from tbl in await repository.GetAllAsync()
										  select tbl;

			if (items.Any())
			{
				this.Logger.LogDebug($"There were {items.Count()} Invoice items retrieved.");

				foreach (IInvoice invoice in items)
				{
					string paid = invoice.Paid ? "Yes" : "No";
					this.Logger.LogInformation("[{id}] {number}, Description = '{description}', Total = {total}, Paid = {paid}", invoice.Id, invoice.Number, invoice.Description, invoice.Total.ToString("$#,##0.00"), invoice.Paid ? "Yes" : "No");
				}
			}
			else
			{
				this.Logger.LogInformation("There are no invoices in the ERP system.");
				returnValue = 1;
			}

			return returnValue;
		}
	}
}

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
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class ListCommand : Command
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		public ListCommand(ILogger<ListCommand> logger, IRepositoryFactory repositoryFactory)
			: base("list", "List all invoices in the data storage.")
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;

			this.Handler = CommandHandler.Create<string>(async _ =>
			{
				return await this.OnHandleCommand();
			});
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<ListCommand> Logger { get; set; } = new NullLogger<ListCommand>();

		/// <summary>
		/// 
		/// </summary>
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected async Task<int> OnHandleCommand()
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

				foreach (IInvoice item in items)
				{
					string paid = item.Paid ? "Yes" : "No";
					this.Logger.LogInformation($"[{item.Id}] {item.Number}, Description = '{item.Description}', Total = {item.Total:$#,##0.00}, Paid = {paid}");
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

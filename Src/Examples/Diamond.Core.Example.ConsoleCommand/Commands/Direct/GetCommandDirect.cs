//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.Performance;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class GetCommandDirect : GetCommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		public GetCommandDirect(ILogger<GetCommandDirect> logger, IRepositoryFactory repositoryFactory, IMeasureAction measureAction)
			: base(logger)
		{
			this.RepositoryFactory = repositoryFactory;
			this.Action = measureAction;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected IMeasureAction Action { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoice"></param>
		/// <returns></returns>
		protected override async Task<int> OnHandleCommand(InvoiceNumber invoice)
		{
			int returnValue = 0;

			//
			// Get a read-only repository for IInvoice.
			//
			this.Logger.LogDebug("Retrieving a read-only repository for IInvoice.");
			IReadOnlyRepository<IInvoice> repository = await this.RepositoryFactory.GetReadOnlyAsync<IInvoice>();

			//
			// Attempt to create the item.
			//
			IInvoice item = await this.Action.Measure(async () => (await repository.GetAsync(t => t.Number == invoice.Number)).SingleOrDefault(), "Get Invoice");
			
			if (item != null)
			{
				returnValue = 0;
				this.Logger.LogInformation("[{id}] {number}, Description = '{description}', Total = {total}, Paid = {paid}", item.Id, item.Number, item.Description, item.Total.ToString("$#,##0.00"), item.Paid ? "Yes" : "No");
			}
			else
			{
				this.Logger.LogError("An invoice with invoice number '{invoice}' could not be found.", invoice.Number);
			}

			return returnValue;
		}
	}
}
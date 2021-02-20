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
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class DeleteCommandHttp : DeleteCommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		public DeleteCommandHttp(ILogger<DeleteCommandHttp> logger, IHttpClientFactory repositoryFactory)
			: base(logger)
		{
			this.HttpClientFactory = repositoryFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IHttpClientFactory HttpClientFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoice"></param>
		/// <returns></returns>
		protected override async Task<int> OnHandleCommand(InvoiceNumber invoice)
		{
			int returnValue = 0;

			HttpClient client = this.HttpClientFactory.CreateClient(typeof(Invoice).Name);

			using (HttpResponseMessage response = await client.DeleteAsync(invoice.Number))
			{
				string responseJson = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					Invoice deletedInvoice = JsonSerializer.Deserialize<Invoice>(responseJson);
					this.Logger.LogInformation($"Successfully deleted invoice: '{deletedInvoice}'.");
				}
				else
				{
					if (response.StatusCode == HttpStatusCode.NotFound)
					{
						this.Logger.LogWarning($"The invoice '{invoice.Number}' was not found.");
					}
					else
					{
						ProblemDetails details = JsonSerializer.Deserialize<ProblemDetails>(responseJson);
						this.Logger.LogError($"Error while deleting invoice '{invoice.Number}': '{details.Detail}'.");
					}

					returnValue = 1;
				}
			}

			return returnValue;
		}
	}
}

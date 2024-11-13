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
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdateCommandHttp : UpdateCommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="httpClientFactory"></param>
		/// <param name="mapper"></param>
		public UpdateCommandHttp(ILogger<UpdateCommandHttp> logger, IHttpClientFactory httpClientFactory)
			: base(logger)
		{
			this.HttpClientFactory = httpClientFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IHttpClientFactory HttpClientFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name=invoice"></param>
		/// <returns></returns>
		protected override async Task<int> OnHandleCommand(Invoice invoice)
		{
			int returnValue = 0;

			HttpClient client = this.HttpClientFactory.CreateClient(typeof(Invoice).Name);

			JsonPatchDocument<Invoice> patchDocument = new JsonPatchDocument<Invoice>();
			string requestJson = JsonSerializer.Serialize(patchDocument);

			using (StringContent content = new(requestJson, Encoding.UTF8, "application/json-patch+json"))
			{
				using (HttpResponseMessage response = await client.PatchAsync(invoice.Number, content))
				{
					string responseJson = await response.Content.ReadAsStringAsync();

					if (response.IsSuccessStatusCode)
					{
						Invoice newInvoice = JsonSerializer.Deserialize<Invoice>(responseJson);
						this.Logger.LogInformation("Successfully updated invoice: '{newInvoice}'.", newInvoice);
					}
					else
					{
						ProblemDetails details = JsonSerializer.Deserialize<ProblemDetails>(responseJson);
						this.Logger.LogError("Error while updating invoice '{invoice}': '{details}'.", invoice.Number, details.Detail);
						returnValue = 1;
					}
				}
			}

			return returnValue;
		}
	}
}

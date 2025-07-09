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
using Diamond.Core.AspNetCore.DoAction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Provides a base class for controllers that handle data table operations with support for both form and JSON
	/// requests.
	/// </summary>
	/// <remarks>This abstract class is designed to facilitate the creation of controllers that manage data table
	/// operations. It provides two endpoints for handling data table requests: one for form submissions and another for
	/// JSON payloads.</remarks>
	/// <typeparam name="TViewModel">The type of the view model used in the data table results.</typeparam>
	/// <typeparam name="TRequest">The type of the request object used for JSON requests.</typeparam>
	[ApiController]
	[Route("[controller]")]
	public abstract class DataTableControllerTemplate<TViewModel, TRequest> : DoActionController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataTableControllerTemplate{TViewModel, TRequest}"/> class.
		/// </summary>
		/// <param name="doActionFactory">The factory used to create actions for data operations.</param>
		/// <param name="logger">The logger instance used for logging operations within the controller.</param>
		public DataTableControllerTemplate(IDoActionFactory doActionFactory, ILogger<DataTableControllerTemplate<TViewModel, TRequest>> logger)
			: base(doActionFactory, logger)
		{
		}

		/// <summary>
		/// Gets or sets the prefix used for keys in the services collection.
		/// </summary>
		protected virtual string KeyPrefix { get; set; } = null;

		/// <summary>
		/// Processes a DataTable request asynchronously using form data.
		/// </summary>
		/// <remarks>This method expects the request to be in the form of
		/// "application/x-www-form-urlencoded".</remarks>
		/// <param name="request">The form data collection containing parameters for the DataTable request.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ActionResult{T}"/> of
		/// type <see cref="DataTableResult{TViewModel}"/> representing the processed DataTable result.</returns>
		[HttpPost("form")]
		[Consumes("application/x-www-form-urlencoded")]
		public virtual Task<ActionResult<DataTableResult<TViewModel>>> DataTableAsync([FromForm] IFormCollection request)
		{
			return this.Do<IFormCollection, DataTableResult<TViewModel>>(request, $"{this.KeyPrefix}{nameof(DataTableAsync)}");
		}

		/// <summary>
		/// Processes a data table request asynchronously and returns the result.
		/// </summary>
		/// <remarks>This method is designed to handle HTTP POST requests with a JSON payload. It consumes the
		/// request, processes it  according to the specified <typeparamref name="TRequest"/> type, and returns a data table
		/// result encapsulated  in an <see cref="ActionResult{T}"/>. Ensure that the request object is correctly formatted
		/// and contains all  necessary parameters for successful processing.</remarks>
		/// <param name="request">The request object containing parameters for the data table operation.  This must be a valid JSON object that
		/// conforms to the expected structure for <typeparamref name="TRequest"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ActionResult{T}"/>  with
		/// a <see cref="DataTableResult{TViewModel}"/> representing the processed data table.</returns>
		[HttpPost("data")]
		[Consumes("application/json")]
		public virtual Task<ActionResult<DataTableResult<TViewModel>>> DataTableAsync([FromBody] TRequest request)
		{
			return this.Do<TRequest, DataTableResult<TViewModel>>(request, $"{this.KeyPrefix}{nameof(DataTableAsync)}");
		}
	}
}

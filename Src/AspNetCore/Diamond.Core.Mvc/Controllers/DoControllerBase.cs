// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.AspNet.DoAction
{
	/// <summary>
	/// Provides the base class for a "Do Action" controller where the action
	/// of the controller method is delegated to a DoAction handler that is
	/// registered in the container using the name of the controller method.
	/// </summary>
	public abstract class DoActionControllerBase
		: ControllerBase
	{
		/// <summary>
		/// Initializes an instance of <see cref="DoActionControllerBase"/> with
		/// an instance of the <see cref="IDoActionFactory"/>.
		/// </summary>
		/// <param name="doActionFactory">An instance of the <see cref="IDoActionFactory"/>.</param>
		public DoActionControllerBase(IDoActionFactory doActionFactory)
		{
			this.DoActionFactory = doActionFactory;
		}

		/// <summary>
		/// Gets/sets the instance of <see cref="ILogger"/> that
		/// will listen for logs events originating from this instance.
		/// </summary>
		public ILogger<DoActionControllerBase> Logger { get; set; } = new NullLogger<DoActionControllerBase>();

		/// <summary>
		/// Gets/sets an instance of <see cref="IDoActionFactory"/>.
		/// </summary>
		protected IDoActionFactory DoActionFactory { get; set; }

		/// <summary>
		/// Executes the controller method without any parameters.
		/// </summary>
		/// <typeparam name="TResult">The type of object returned by the action.</typeparam>
		/// <param name="actionKey">The name of the action retrieved from the container.</param>
		/// <returns>An ActionResult encapsulating the expected return type.</returns>
		protected Task<ActionResult<TResult>> Do<TResult>(string actionKey)
		{
			this.Logger.LogTrace($"Do method called with action key '{actionKey}'.");
			return this.Do<object, TResult>(actionKey, null);
		}

		/// <summary>
		/// Executes the controller method without the given parameter.
		/// </summary>
		/// <typeparam name="TInputs"></typeparam>
		/// <typeparam name="TResult">The type of object returned by the action.</typeparam>
		/// <param name="actionKey">The name of the action retrieved from the container.</param>
		/// <param name="request">The input parameter for the action.</param>
		/// <returns>An ActionResult encapsulating the expected return type.</returns>
		protected async Task<ActionResult<TResult>> Do<TInputs, TResult>(string actionKey, TInputs request)
		{
			ActionResult<TResult> returnValue = default;

			try
			{
				// ***
				// *** Get the IDoAction
				// ***
				IDoAction<TInputs, IControllerActionResult<TResult>> action = null;

				try
				{
					this.Logger.LogTrace($"Retrieving controller method action '{actionKey}'.");
					action = await this.DoActionFactory.GetAsync<TInputs, IControllerActionResult<TResult>>(actionKey);
					this.Logger.LogTrace($"Controller method action '{actionKey}' was successfully retrieved.");
				}
				catch (DoActionNotFoundException<TInputs, TResult>)
				{
					// ***
					// *** An implementation of this method was not found.
					// ***
					this.Logger.LogWarning($"Controller method action '{actionKey}' was not found in the container.");
					returnValue = this.StatusCode(StatusCodes.Status501NotImplemented);
					action = null;
				}
				catch (Exception ex)
				{
					// ***
					// *** An implementation of this method was not found.
					// ***
					this.Logger.LogError(ex, $"Exception while retrieving do action.");
					returnValue = this.StatusCode(StatusCodes.Status500InternalServerError);
					action = null;
				}

				if (action != null)
				{
					using (ITryDisposable<IDoAction<TInputs, IControllerActionResult<TResult>>> disposable = new TryDisposable<IDoAction<TInputs, IControllerActionResult<TResult>>>(action))
					{
						// ***
						// *** Execute the action.
						// ***
						this.Logger.LogTrace($"Executing controller method action '{actionKey}.TakeActionAsync()'.");
						IControllerActionResult<TResult> result = await action.ExecuteActionAsync(request);

						if (result.ResultType == ResultType.Ok)
						{
							this.Logger.LogTrace($"Controller method action '{actionKey}.TakeActionAsync()' completed successfully.");
							returnValue = this.Ok(result.Result);
						}
						else if (result.ResultType == ResultType.BadRequest)
						{
							this.Logger.LogTrace($"Controller method action '{actionKey}.TakeActionAsync()' completed with 'ResultType.BadRequest'.");
							returnValue = this.BadRequest(new FailedRequest("Bad Request", result.ErrorMessage));
						}
						else if (result.ResultType == ResultType.NotFound)
						{
							this.Logger.LogTrace($"Controller method action '{actionKey}.TakeActionAsync()' completed with 'ResultType.NotFound'.");
							returnValue = this.NotFound(new FailedRequest("Not Found", result.ErrorMessage));
						}
					}
				}
			}
			catch (Exception ex)
			{
				// ***
				// *** Internal error.
				// ***
				this.Logger.LogError(ex, nameof(Do));
				returnValue = this.StatusCode(StatusCodes.Status500InternalServerError);
			}

			return returnValue;
		}

		/// <summary>
		/// Logs a method call.
		/// </summary>
		/// <param name="name"></param>
		protected void LogMethodCall([CallerMemberName] string name = null)
		{
			this.Logger.LogTrace($"Controller method '{name}' was called.");
		}
	}
}

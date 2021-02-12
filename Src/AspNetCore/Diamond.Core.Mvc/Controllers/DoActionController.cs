﻿//
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
	public abstract class DoActionController
		: ControllerBase
	{
		/// <summary>
		/// Initializes an instance of <see cref="DoActionController"/> with
		/// an instance of the <see cref="IDoActionFactory"/>.
		/// </summary>
		/// <param name="doActionFactory">An instance of the <see cref="IDoActionFactory"/>.</param>
		public DoActionController(IDoActionFactory doActionFactory)
		{
			this.DoActionFactory = doActionFactory;
		}

		/// <summary>
		/// Initializes an instance of <see cref="DoActionController"/> with
		/// an instance of the <see cref="IDoActionFactory"/> and the <see cref="ILogger"/>.
		/// </summary>
		/// <param name="doActionFactory"></param>
		/// <param name="logger"></param>
		public DoActionController(IDoActionFactory doActionFactory, ILogger<DoActionController> logger)
		{
			this.DoActionFactory = doActionFactory;
			this.Logger = logger;
		}

		/// <summary>
		/// Gets/sets the instance of <see cref="ILogger"/> that
		/// will listen for logs events originating from this instance.
		/// </summary>
		public virtual ILogger<DoActionController> Logger { get; set; } = new NullLogger<DoActionController>();

		/// <summary>
		/// Gets/sets an instance of <see cref="IDoActionFactory"/>.
		/// </summary>
		protected virtual IDoActionFactory DoActionFactory { get; set; }

		/// <summary>
		/// Executes the controller method without any parameters.
		/// </summary>
		/// <typeparam name="TResult">The type of object returned by the action.</typeparam>
		/// <param name="actionKey">The name of the action retrieved from the container.</param>
		/// <returns>An ActionResult encapsulating the expected return type.</returns>
		protected virtual Task<ActionResult<TResult>> Do<TResult>([CallerMemberName] string actionKey = null)
		{
			this.Logger.LogTrace($"Do method called with action key '{actionKey}'.");
			return this.Do<object, TResult>(null, actionKey);
		}

		/// <summary>
		/// Executes the controller method without the given parameter.
		/// </summary>
		/// <typeparam name="TInputs"></typeparam>
		/// <typeparam name="TResult">The type of object returned by the action.</typeparam>
		/// <param name="actionKey">The name of the action retrieved from the container.</param>
		/// <param name="inputs">The input parameter for the action.</param>
		/// <returns>An ActionResult encapsulating the expected return type.</returns>
		protected virtual async Task<ActionResult<TResult>> Do<TInputs, TResult>(TInputs inputs, [CallerMemberName] string actionKey = null)
		{
			ActionResult<TResult> returnValue = default;

			try
			{
				//
				// Get the IDoAction
				//
				IDoAction<TInputs, TResult> action = null;

				try
				{
					this.Logger.LogTrace($"Retrieving controller method action '{actionKey}'.");
					action = await this.DoActionFactory.GetAsync<TInputs, TResult>(actionKey);
					this.Logger.LogTrace($"Controller method action '{actionKey}' was successfully retrieved.");
				}
				catch (DoActionNotFoundException)
				{
					//
					// An implementation of this method was not found.
					//
					this.Logger.LogWarning($"Controller method action '{actionKey}' was not found in the container.");
					returnValue = this.StatusCode(StatusCodes.Status501NotImplemented, this.OnCreateProblemDetail(DoActionResult.NotImplemented($"The method has not been implemented. This could be a configuration error or the service is still under development.")));
					action = null;
				}
				catch (Exception ex)
				{
					//
					// An implementation of this method was not found.
					//
					this.Logger.LogError(ex, $"Exception while retrieving do action.");
					returnValue = this.StatusCode(StatusCodes.Status500InternalServerError, this.OnCreateProblemDetail(DoActionResult.InternalServerError("An unknown internal server error occurred.")));
					action = null;
				}

				if (action != null)
				{
					using (ITryDisposable<IDoAction<TInputs, TResult>> disposable = new TryDisposable<IDoAction<TInputs, TResult>>(action))
					{
						//
						// Perform the extra moodel validation step.
						//
						(bool modelResult, string errorMessage) = await action.ValidateModel(inputs);

						if (modelResult)
						{
							//
							// Execute the action.
							//
							this.Logger.LogTrace($"Executing controller method action '{actionKey}.TakeActionAsync()'.");
							IControllerActionResult<TResult> result = await action.ExecuteActionAsync(inputs);

							if (result.ResultDetails.Status == StatusCodes.Status200OK)
							{
								this.Logger.LogTrace($"Controller method action '{actionKey}.TakeActionAsync()' completed successfully.");
								returnValue = this.Ok(result.Result);
							}
							else
							{
								this.Logger.LogTrace($"Controller method action '{actionKey}.TakeActionAsync()' completed with HTTP Status Code of {result.ResultDetails.Status}.");
								this.Logger.LogTrace($"The action returned: '{result.ResultDetails.Detail}'.");

								//
								// Check if the instance is null.
								//
								if (result.ResultDetails.Instance == null)
								{
									//
									// Add the request path.
									//
									result.ResultDetails.Instance = HttpContext.Request.Path;
								}

								returnValue = this.BadRequest(this.OnCreateProblemDetail(result.ResultDetails));
							}
						}
						else
						{
							//
							// Model validation failed. Return a 400
							//
							ProblemDetails problemDetails = DoActionResult.BadRequest(errorMessage);
							returnValue = this.BadRequest(this.OnCreateProblemDetail(problemDetails));
						}
					}
				}
			}
			catch (Exception ex)
			{
				//
				// Internal error.
				//
				this.Logger.LogError(ex, nameof(Do));
				returnValue = this.StatusCode(StatusCodes.Status500InternalServerError);
			}

			return returnValue;
		}

		/// <summary>
		/// Logs a method call.
		/// </summary>
		/// <param name="name"></param>
		protected virtual void LogMethodCall([CallerMemberName] string name = null)
		{
			this.Logger.LogTrace($"Controller method '{name}' was called.");
		}

		/// <summary>
		/// Provides the overriding class the opportunity to edit or change the problems
		/// details before they are returned to the client.
		/// </summary>
		/// <param name="problemDetails">The instance of <see cref="ProblemDetails"/> that will be returned to the client.</param>
		/// <returns>An instance of <see cref="ProblemDetails"/>.</returns>
		protected virtual ProblemDetails OnCreateProblemDetail(ProblemDetails problemDetails)
		{
			//
			// TO DO: Add code here to override details...
			//

			return problemDetails;
		}
	}
}

// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
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
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Diamond.Patterns.Mvc.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.Patterns.Mvc
{
	/// <summary>
	/// Provides the base class for a "Do" controller where the action
	/// of the controller method is delegated to a Decorator that is
	/// registered in the container using the name of the controller method.
	/// </summary>
	public abstract class DoControllerBase : ControllerBase, ILogger
	{
		/// <summary>
		/// Initializes an instance of <see cref="DoControllerBase"/> with
		/// an instance of the <see cref="IDecoratorFactory"/>.
		/// </summary>
		/// <param name="decoratorFactory">An instance of the <see cref="IDecoratorFactory"/>.</param>
		public DoControllerBase(IDecoratorFactory decoratorFactory)
		{
			this.DecoratorFactory = decoratorFactory;
		}

		/// <summary>
		/// Initializes an instance of <see cref="DoControllerBase"/> with
		/// an instance of the <see cref="IDecoratorFactory"/>.
		/// </summary>
		/// <param name="decoratorFactory">An instance of the <see cref="IDecoratorFactory"/>.</param>
		/// <param name="loggerSubscriber">An instance of the <see cref="ILoggerSubscriber"/>.</param>
		public DoControllerBase(IDecoratorFactory decoratorFactory, ILoggerSubscriber loggerSubscriber)
		{
			this.DecoratorFactory = decoratorFactory;
			this.LoggerSubscriber = loggerSubscriber;
		}

		/// <summary>
		/// Gets/sets the instance of <see cref="ILoggerSubscriber"/> that
		/// will listen for logs events originating from this instance.
		/// </summary>
		public ILoggerSubscriber LoggerSubscriber { get; set; }

		/// <summary>
		/// Gets/sets an instance of <see cref="IDecoratorFactory"/>.
		/// </summary>
		protected IDecoratorFactory DecoratorFactory { get; set; }

		/// <summary>
		/// Executes the controller method without any parameters.
		/// </summary>
		/// <typeparam name="TResult">The type of object returned by the action.</typeparam>
		/// <param name="decoratorName">The name of the decorator retrieved from the container.</param>
		/// <returns>An ActionResult encapsulating the expected return type.</returns>
		protected Task<ActionResult<TResult>> Do<TResult>(string decoratorName)
		{
			return this.Do<object, TResult>(decoratorName, null);
		}

		/// <summary>
		/// Executes the controller method without the given parameter.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <typeparam name="TResult">The type of object returned by the action.</typeparam>
		/// <param name="decoratorName">The name of the decorator retrieved from the container.</param>
		/// <param name="request">The input parameter for the action.</param>
		/// <returns>An ActionResult encapsulating the expected return type.</returns>
		protected async Task<ActionResult<TResult>> Do<TItem, TResult>(string decoratorName, TItem request)
		{
			ActionResult<TResult> returnValue = default;

			try
			{
				// ***
				// *** Request a decorator that adds and action to an object of type Tuple<int, int>
				// *** and returns a instance of IRateCard when the action is executed.
				// ***
				IDecorator<TItem, IControllerActionResult<TResult>> decorator = null;

				try
				{
					this.LoggerSubscriber.Verbose($"Retrieving controller method decorator '{decoratorName}'.");
					decorator = await this.DecoratorFactory.GetAsync<TItem, IControllerActionResult<TResult>>(decoratorName);
					this.LoggerSubscriber.Verbose($"Controller method decorator '{decoratorName}' was successfully retrieved.");
				}
				catch
				{
					// ***
					// *** An implementation of this method was not found.
					// ***
					this.LoggerSubscriber.Warning($"Controller method decorator '{decoratorName}' was not found in the container.");
					returnValue = this.StatusCode(StatusCodes.Status501NotImplemented);
					decorator = null;
				}

				if (decorator != null)
				{
					using (ITryDisposable<IDecorator<TItem, IControllerActionResult<TResult>>> disposable = new TryDisposable<IDecorator<TItem, IControllerActionResult<TResult>>>(decorator))
					{
						// ***
						// *** Execute the decorator action.
						// ***
						this.LoggerSubscriber.Verbose($"Executing controller method decorator '{decoratorName}.TakeActionAsync()'.");
						IControllerActionResult<TResult> result = await decorator.TakeActionAsync(request);

						if (result.ResultType == ResultType.Ok)
						{
							this.LoggerSubscriber.Verbose($"Controller method decorator '{decoratorName}.TakeActionAsync()' completed successfully.");
							returnValue = this.Ok(result.Result);
						}
						else if (result.ResultType == ResultType.BadRequest)
						{
							this.LoggerSubscriber.Verbose($"Controller method decorator '{decoratorName}.TakeActionAsync()' completed with 'ResultType.BadRequest'.");
							returnValue = this.BadRequest(new FailedRequest("Bad Request", result.ErrorMessage));
						}
						else if (result.ResultType == ResultType.NotFound)
						{
							this.LoggerSubscriber.Verbose($"Controller method decorator '{decoratorName}.TakeActionAsync()' completed with 'ResultType.NotFound'.");
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
				this.LoggerSubscriber.Exception(ex);
				returnValue = this.StatusCode(StatusCodes.Status500InternalServerError);
			}

			return returnValue;
		}
	}
}

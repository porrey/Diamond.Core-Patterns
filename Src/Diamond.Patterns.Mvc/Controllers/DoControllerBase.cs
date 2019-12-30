using System;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Diamond.Patterns.Mvc.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace Diamond.Patterns.Mvc
{
	public abstract class DoControllerBase : ControllerBase
	{
		public DoControllerBase(IDecoratorFactory decoratorFactory)
		{
			this.DecoratorFactory = decoratorFactory;
		}

		protected IDecoratorFactory DecoratorFactory { get; set; }

		protected Task<ActionResult<TResult>> Do<TResult>(string decoratorName)
		{
			return this.Do<object, TResult>(decoratorName, null);
		}

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
					decorator = await this.DecoratorFactory.GetAsync<TItem, IControllerActionResult<TResult>>(decoratorName);
				}
				catch
				{
					decorator = null;
				}

				// ***
				// ***
				// ***
				if (decorator != null)
				{
					using (ITryDisposable<IDecorator<TItem, IControllerActionResult<TResult>>> disposable = new TryDisposable<IDecorator<TItem, IControllerActionResult<TResult>>>(decorator))
					{
						// ***
						// *** Execute the decorator action.
						// ***
						IControllerActionResult<TResult> result = await decorator.TakeActionAsync(request);

						if (result.ResultType == ResultType.Ok)
						{
							returnValue = this.Ok(result.Result);
						}
						else if (result.ResultType == ResultType.BadRequest)
						{
							returnValue = this.BadRequest(new FailedRequest("Bad Request", result.ErrorMessage));
						}
						else if (result.ResultType == ResultType.NotFound)
						{
							returnValue = this.NotFound(new FailedRequest("Not Found", result.ErrorMessage));
						}
					}
				}
				else
				{
					// ***
					// *** An implementation of this method was not found.
					// ***
					returnValue = this.StatusCode(StatusCodes.Status501NotImplemented);
				}
			}
			catch (Exception ex)
			{
				// ***
				// *** Internal error.
				// ***
				Console.WriteLine(ex.Message);
				returnValue = this.StatusCode(StatusCodes.Status500InternalServerError);
			}

			return returnValue;
		}
	}
}

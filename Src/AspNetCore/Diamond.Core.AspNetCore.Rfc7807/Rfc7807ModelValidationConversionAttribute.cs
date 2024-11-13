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
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Diamond.Core.AspNetCore.Rfc7807
{
	/// <summary>
	/// Updates the default problem details object returned from the model validation to
	/// conform with RFC 7807.
	/// </summary>
	public class Rfc7807ModelValidationConversionAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Called by the ASP.NET MVC framework after the action result executes.
		/// </summary>
		/// <param name="context">Provides the context for the OnResultExecuted(ResultExecutedContext)
		/// method of an <see cref="ActionFilterAttribute"/> class.</param>
		public override void OnResultExecuting(ResultExecutingContext context)
		{
			//
			// Check if the model state is valid.
			//
			if (!context.ModelState.IsValid)
			{
				//
				// Cast the controller property from object to ControllerBase.
				//
				if (context.Controller is ControllerBase controllerBase)
				{
					//
					// Get all validation error messages.
					//
					string detail = String.Join(";", context.ModelState.Values
															.SelectMany(t => t.Errors)
															.Select(t => t.ErrorMessage));

					//
					// Check that the result is valid.
					//
					if (context.Result is BadRequestObjectResult result)
					{
						//
						// Check that it is a valid ProblemDetails.
						//
						if (result.Value is ProblemDetails problemDetails)
						{
							//
							// Set the detail error.
							//
							problemDetails.Detail = detail;
						}
					}
				}
				else
				{
					//
					// This scenario should never happen, but since the context is out
					// out of our control, this ensures the response is processed.
					//
					base.OnResultExecuting(context);
				}
			}
			else
			{
				//
				// Allow the process to complete in a normal fashion.
				//
				base.OnResultExecuting(context);
			}
		}
	}
}
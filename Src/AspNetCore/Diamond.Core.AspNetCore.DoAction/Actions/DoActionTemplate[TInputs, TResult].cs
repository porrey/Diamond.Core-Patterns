//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.AspNetCore.DoAction
{
	/// <summary>
	/// A template class for implementing a controller "Do Action". A method within
	/// a controller is delegated to this handler.
	/// </summary>
	public class DoActionTemplate<TInputs, TResult> : IDoAction<TInputs, TResult>
	{
		/// <summary>
		/// Create an instance of <see cref="DoActionTemplate{TInputs, TResult}"/> with the
		/// specified logger instance.
		/// </summary>
		/// <param name="logger">The logger instance used by this object.</param>
		public DoActionTemplate(ILogger<DoActionTemplate<TInputs, TResult>> logger)
			: this()
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Create a default instance of <see cref="DoActionTemplate{TInputs, TResult}"/>.
		/// </summary>
		public DoActionTemplate()
		{
			this.ActionKey = this.GetType().Name.Replace("Action", "");
		}

		/// <summary>
		/// Gets/sets the instance of the logger used by the factory. The default is a null logger.
		/// </summary>
		protected virtual ILogger<DoActionTemplate<TInputs, TResult>> Logger { get; set; } = new NullLogger<DoActionTemplate<TInputs, TResult>>();

		/// <summary>
		/// Gets the unique key that identifies this action. As a best practice, the name
		/// of this class should match the controller method name with the word "Action" 
		/// appended to the end. The DoActionController uses [CallerMemberName] when calling the
		/// factory to retrieve the action. The controller will automatically remove the "Action"
		/// term and matches it to this property. The default implementation of this property returns
		/// the name of the class without the "action" term..
		/// </summary>
		public virtual string ActionKey { get; set; }

		/// <summary>
		/// Executes the controller method action returning the result or an error with an HTTP status code. The default
		/// implementation calls the OnExecuteActionAsync() method.
		/// </summary>
		/// <param name="item">The inputs passed to the controller method. To support multiple
		/// parameters use a Tuple.</param>
		/// <returns>A <see cref="IControllerActionResult{TResult}"/> instance that contains the desired
		/// result with an HTTP status of 2xx or an error and the appropriate HTTP error status code.</returns>
		public virtual Task<IControllerActionResult<TResult>> ExecuteActionAsync(TInputs item)
		{
			return this.OnExecuteActionAsync(item);
		}

		/// <summary>
		/// Executes the controller method action returning the result or an error with an HTTP status code. Override this
		/// method to perform the action.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual Task<IControllerActionResult<TResult>> OnExecuteActionAsync(TInputs item)
		{
			return Task.FromResult<IControllerActionResult<TResult>>(new ControllerActionResult<TResult>());
		}

		/// <summary>
		/// Performs a validation of the action inputs prior to the execution. If the methods returns false, the
		/// string will contain a list of error messages.
		/// </summary>
		/// <param name="item">The inputs passed to the controller method.</param>
		/// <returns>Returns true if the model was validated successfully, and false otherwise. The string type will
		/// contain one or more error messages concatenated in a single string.</returns>
		public virtual Task<(bool, string)> ValidateModel(TInputs item)
		{
			return Task.FromResult<(bool, string)>((true, null));
		}
	}
}

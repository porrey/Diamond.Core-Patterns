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
using System.Threading.Tasks;

namespace Diamond.Core.AspNetCore.DoAction
{
	/// <summary>
	/// Defines a generic decorator.
	/// </summary>
	public interface IDoAction
	{
		/// <summary>
		/// Gets/sets the unique name for this action.
		/// </summary>
		string ActionKey { get; }
	}

	/// <summary>
	/// Defines a decorator that can has wraps TItem and
	/// returns TResult.
	/// </summary>
	/// <typeparam name="TInputs">The instance type being decorated.</typeparam>
	/// <typeparam name="TResult">The result of the decorator TakeActionAsync method.</typeparam>
	public interface IDoAction<TInputs, TResult> : IDoAction
	{
		/// <summary>
		/// Performs a validation of the action inputs prior to the execution. If the methods returns false, the
		/// string will contain a list of error messages.
		/// </summary>
		/// <param name="item">The inputs passed to the controller method.</param>
		/// <returns>Returns true if the model was validated successfully, and false otherwise. The string type will
		/// contain one or more error messages concatenated in a single string.</returns>
		Task<(bool, string)> ValidateModel(TInputs item);

		/// <summary>
		/// Executes the controller method action returning the result or an error with an HTTP status code. The default
		/// implementation calls the OnExecuteActionAsync() method.
		/// </summary>
		/// <param name="item">The inputs passed to the controller method. To support multiple
		/// parameters use a Tuple.</param>
		/// <returns>A <see cref="IControllerActionResult{TResult}"/> instance that contains the desired
		/// result with an HTTP status of 2xx or an error and the appropriate HTTP error status code.</returns>
		Task<IControllerActionResult<TResult>> ExecuteActionAsync(TInputs item);
	}
}

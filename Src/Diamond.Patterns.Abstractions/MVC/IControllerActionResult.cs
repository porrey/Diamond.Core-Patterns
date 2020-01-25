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
namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Specifies the result type of a controller action.
	/// </summary>
	public enum ResultType
	{
		/// <summary>
		/// The action result in success (usually a 200 status).
		/// </summary>
		Ok,
		/// <summary>
		/// The action result in not found (usually a 404 status).
		/// </summary>
		NotFound,
		/// <summary>
		/// The action result in bad request (usually a 400 status).
		/// </summary>
		BadRequest
	}

	/// <summary>
	/// Contains the result of a controller action.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IControllerActionResult<TResult>
	{
		/// <summary>
		/// The type of response usually associated to an HTTP status code.
		/// </summary>
		ResultType ResultType { get; set; }
		/// <summary>
		/// A description of the error if the action failed.
		/// </summary>
		string ErrorMessage { get;  }
		/// <summary>
		/// The resulting object instance if the action was successful.
		/// </summary>
		TResult Result { get; }
	}
}

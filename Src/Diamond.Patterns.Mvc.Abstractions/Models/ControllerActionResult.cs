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
using Diamond.Patterns.Abstractions;

#pragma warning disable CS1591

namespace Diamond.Patterns.Mvc.Abstractions
{
	/// <summary>
	/// Contains the result of a controller action.
	/// </summary>
	/// <typeparam name="TResult">The type of the inner object.</typeparam>
	public class ControllerActionResult<TResult> : IControllerActionResult<TResult>
	{
		/// <summary>
		/// The type of response usually associated to an HTTP status code.
		/// </summary>
		public ResultType ResultType { get; set; }

		/// <summary>
		/// A description of the error if the action failed.
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// The resulting object instance if the action was successful.
		/// </summary>
		public TResult Result { get; set; }
	}
}

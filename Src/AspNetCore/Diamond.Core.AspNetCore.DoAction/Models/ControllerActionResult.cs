//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using Microsoft.AspNetCore.Mvc;

namespace Diamond.Core.AspNetCore.DoAction
{
	/// <summary>
	/// Contains the result of a controller <see cref="DoActionTemplate{TInputs, TResult}"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of the inner object.</typeparam>
	public class ControllerActionResult<TResult> : IControllerActionResult<TResult>
	{
		/// <summary>
		/// The instance of <see cref="ProblemDetails"/> that is returned to the caller
		/// if the result is not a 200.
		/// </summary>
		public virtual ProblemDetails ResultDetails { get; set; }

		/// <summary>
		/// The resulting object instance if the action was successful.
		/// </summary>
		public virtual TResult Result { get; set; }
	}
}

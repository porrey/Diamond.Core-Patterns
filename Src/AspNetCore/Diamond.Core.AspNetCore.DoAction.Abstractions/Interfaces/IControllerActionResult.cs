//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
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
	/// Contains the result of a controller action.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IControllerActionResult<TResult>
	{
		/// <summary>
		/// The instance of <see cref="ProblemDetails"/> that is returned to the caller
		/// if the result is not 2xx.
		/// </summary>
		ProblemDetails ResultDetails { get; set; }

		/// <summary>
		/// The resulting object instance if the action was successful.
		/// </summary>
		TResult Result { get; }
	}
}

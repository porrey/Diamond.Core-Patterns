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

namespace Diamond.Core.AspNetCore.DoAction
{
	/// <summary>
	/// An exception indicating that a specified DoAction was not configured. This
	/// usually results in an HTTP 501 status.
	/// </summary>
	public class DoActionNotFoundException : DiamondDoActionException
	{
		/// <summary>
		/// Creates an instance of <see cref="DoActionNotFoundException"/> specifying the
		/// input type, the result type and the unique action key.
		/// </summary>
		/// <param name="tinputs">The type of inputs for the missing DoAction.</param>
		/// <param name="tresult">The result type of the missing DoAction.</param>
		/// <param name="actionKey">The unique key used to locate the DoAction in the container.</param>
		public DoActionNotFoundException(Type tinputs, Type tresult, string actionKey)
			: base($"A do action of type 'IDoAction<{tinputs.Name}, {tresult.Name}>' named '{actionKey}' has not been configured.")
		{
		}
	}
}

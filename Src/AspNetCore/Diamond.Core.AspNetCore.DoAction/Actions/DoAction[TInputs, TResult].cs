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
using System;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.AspNetCore.DoAction
{
	/// <summary>
	/// This class has been deprecated and should not be used. The <see cref="DoActionTemplate{TInputs, TResult}"/>
	/// class should be used instead.
	/// </summary>
	[Obsolete("Use DoActionTemplate instead.")]
	public class DoAction<TInputs, TResult> : DoActionTemplate<TInputs, TResult>
	{
		/// <summary>
		/// Creates an instance of <see cref="DoAction{TInputs, TResult}"/> with the specified logger instance.
		/// </summary>
		/// <param name="logger">An instance of a logger.</param>
		public DoAction(ILogger<DoAction<TInputs, TResult>> logger)
			: base(logger)
		{
			this.Logger = logger;
		}
	}
}

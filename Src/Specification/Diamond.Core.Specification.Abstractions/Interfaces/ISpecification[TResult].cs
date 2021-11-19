﻿//
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

namespace Diamond.Core.Specification
{
	/// <summary>
	/// Defines a specification that selects objects without any inputs.
	/// </summary>
	/// <typeparam name="TResult">The type of the result when the selection executes.</typeparam>
	public interface ISpecification<TResult> : ISpecification
	{
		/// <summary>
		/// Executes the selection by the specification design.
		/// </summary>
		/// <returns>Returns the result as a instance of type TResult.</returns>
		Task<TResult> ExecuteSelectionAsync();
	}
}

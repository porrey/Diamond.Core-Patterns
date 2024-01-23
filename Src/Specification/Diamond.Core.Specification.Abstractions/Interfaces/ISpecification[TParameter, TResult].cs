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
using System.Threading.Tasks;

namespace Diamond.Core.Specification
{
	/// <summary>
	/// Defines a specification that takes input(s) of type TParameter and returns a selection of type TResult.
	/// </summary>
	/// <typeparam name="TParameter">The type of input(s) required for the selection.</typeparam>
	/// <typeparam name="TResult">The type of the result when the selection executes.</typeparam>
	public interface ISpecification<TParameter, TResult> : ISpecification
	{
		/// <summary>
		/// Executes the selection by the specification design.
		/// </summary>
		/// <param name="inputs">Specifies the inputs used as the selection criteria. To specify more
		/// than one value, use a Tuple for TParameter.</param>
		/// <returns>Returns the result of the selection as type TResult.</returns>
		Task<TResult> ExecuteSelectionAsync(TParameter inputs);
	}
}

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
namespace Diamond.Core.Specification
{
	/// <summary>
	/// An exception indicating that an <see cref="SpecificationNotFoundException{TParameter, TResult}" /> was not found.
	/// </summary>
	/// <typeparam name="TParameter">The required input type for the specification that was not found.</typeparam>
	/// <typeparam name="TResult">The expected result of the specification that was not found.</typeparam>
	public class SpecificationNotFoundException<TParameter, TResult> : DiamondSpecificationException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public SpecificationNotFoundException(string name)
			: base($"A Specification of type 'ISpecification<{typeof(TParameter).Name}, {typeof(TResult).Name}>' with name '{name}' has not been configured.")
		{
		}
	}
}

//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.Abstractions;

namespace Diamond.Core.Specification
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public class SpecificationNotFoundException<TResult> : DiamondCoreException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public SpecificationNotFoundException(string name)
			: base($"A Specification of type 'ISpeciification<{typeof(TResult).Name}>' with name '{name}' has not been configured.")
		{
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TParameter"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public class SpecificationNotFoundException<TParameter, TResult> : DiamondCoreException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public SpecificationNotFoundException(string name)
			: base($"A Specification of type 'ISpeciification<{typeof(TParameter).Name}, {typeof(TResult).Name}>' with name '{name}' has not been configured.")
		{
		}
	}
}

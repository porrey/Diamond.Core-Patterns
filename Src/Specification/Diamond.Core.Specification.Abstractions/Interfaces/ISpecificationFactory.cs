//
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
	/// Defines a factory that can be used to retrieve a specific instance of
	/// <see cref="ISpecification"/> from a container. The scope is always 
	/// controlled by the registration of the specification into the container.
	/// </summary>
	public interface ISpecificationFactory
	{
		/// <summary>
		/// Gets a specification with the given name return type TResult.
		/// </summary>
		/// <typeparam name="TResult">The return type of the specification result.</typeparam>
		/// <param name="name"></param>
		/// <returns>An instance of the requested specification.</returns>
		Task<ISpecification<TResult>> GetAsync<TResult>(string name);

		/// <summary>
		/// Gets a specification with the given name that takes TParameter as inputs and returns
		/// the type TResult.
		/// </summary>
		/// <typeparam name="TParameter">The type of the filter used in the specification.</typeparam>
		/// <typeparam name="TResult">The return type of the specification result.</typeparam>
		/// <param name="name"></param>
		/// <returns>An instance of the requested specification.</returns>
		Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>(string name);
	}
}

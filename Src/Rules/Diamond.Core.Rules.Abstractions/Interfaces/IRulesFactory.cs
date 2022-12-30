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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diamond.Core.Rules
{
	/// <summary>
	/// Defines a factory that can return all rules defined for a specific model
	/// (by type) and optionally a group name.
	/// </summary>
	public interface IRulesFactory
	{
		/// <summary>
		/// Get all model rule instances registered based on TInterface.
		/// </summary>
		/// <returns>A list of <see cref="IRule"/> instances.</returns>
		Task<IEnumerable<IRule<TInterface>>> GetAllAsync<TInterface>();

		/// <summary>
		/// Get all model rule instances registered based on TInterface and group name.
		/// </summary>
		/// <returns>A list of <see cref="IRule"/> instances.</returns>
		Task<IEnumerable<IRule<TInterface>>> GetAllAsync<TInterface>(string group);

		/// <summary>
		/// Get all model rule instances registered based on TInterface and group name.
		/// </summary>
		/// <returns>A list of <see cref="IRule"/> instances.</returns>
		Task<IEnumerable<IRule<TInterface, TResult>>> GetAllAsync<TInterface, TResult>();

		/// <summary>
		/// Get all model rule instances registered based on TInterface and group name.
		/// </summary>
		/// <returns>A list of <see cref="IRule"/> instances.</returns>
		Task<IEnumerable<IRule<TInterface, TResult>>> GetAllAsync<TInterface, TResult>(string group);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="group"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<string> EvaluateAsync<TItem>(string group, TItem item);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<string> EvaluateAsync<TItem>(TItem item);
	}
}

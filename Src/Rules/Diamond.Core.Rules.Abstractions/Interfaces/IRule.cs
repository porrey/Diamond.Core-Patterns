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
using System.Threading.Tasks;

namespace Diamond.Core.Rules
{
	/// <summary>
	/// Interface defining a generic rule.
	/// </summary>
	public interface IRule
	{
		/// <summary>
		/// Group name to distinguish between different rule sets.
		/// </summary>
		string Group { get; set; }
	}

	/// <summary>
	/// Interface defining a generic rule.
	/// </summary>
	/// <typeparam name="TItem">The type of item the rule is applied to.</typeparam>
	/// <typeparam name="TResult">The object type of the result.</typeparam>
	public interface IRule<TItem, TResult> : IRule
	{
		/// <summary>
		/// Validate entity based on the defined rule asynchronously.
		/// </summary>
		/// <param name="item">The item to be validated.</param>
		/// <returns>A boolean value indicating whether or not the rule has been
		/// validated. If false, an error message is returned.</returns>
		Task<TResult> ValidateAsync(TItem item);
	}

	/// <summary>
	/// Interface defining a generic rule.
	/// </summary>
	/// <typeparam name="TItem">The type of item the rule is applied to.</typeparam>
	public interface IRule<TItem> : IRule<TItem, IRuleResult>
	{
	}
}

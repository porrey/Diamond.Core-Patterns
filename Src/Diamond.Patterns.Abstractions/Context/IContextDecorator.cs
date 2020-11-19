// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Defines a decorator for a context that adds a state
	/// dictionary and a reset method.
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	public interface IContextDecorator<TContext> where TContext : IContext
	{
		/// <summary>
		/// Gets the underlying context instance.
		/// </summary>
		TContext Item { get; set; }

		/// <summary>
		/// Resets the context.
		/// </summary>
		Task ResetAsync();

		/// <summary>
		/// Gets/sets properties to be contained  within the context.
		/// </summary>
		IStateDictionary Properties { get; set; }
	}
}

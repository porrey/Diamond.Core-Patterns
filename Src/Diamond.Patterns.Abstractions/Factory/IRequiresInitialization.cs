// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
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
	/// Defines an interfaces to be used to indicate that an object within
	/// a container requires initialization. This is useful when objects are
	/// instantiated by a container but need to be initialized. The default
	/// implementation of <see cref="IObjectFactory"/> checks objects for
	/// this interface and calls InitializeAsync() when CanInitialize returns
	/// true and IsInitialized returns false.
	/// </summary>
	public interface IRequiresInitialization
	{
		/// <summary>
		/// Gets a value indicating if the instance can be initialized.
		/// </summary>
		bool CanInitialize { get; }

		/// <summary>
		/// Gets a value indicating if the instance has been initialized.
		/// </summary>
		bool IsInitialized { get; set; }

		/// <summary>
		/// Initializes the instance.
		/// </summary>
		/// <returns></returns>
		Task<bool> InitializeAsync();
	}
}

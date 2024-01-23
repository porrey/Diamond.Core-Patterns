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

namespace Diamond.Core.Repository
{
	/// <summary>
	/// Defines a factory to create entity models.
	/// </summary>
	/// <typeparam name="TInterface">The type of the entity model.</typeparam>
	public interface IEntityFactory<TInterface>
	{
		/// <summary>
		/// Creates a new empty instance of an entity model.
		/// </summary>
		/// <returns>The newly created entity.</returns>
		Task<TInterface> CreateAsync();
	}
}

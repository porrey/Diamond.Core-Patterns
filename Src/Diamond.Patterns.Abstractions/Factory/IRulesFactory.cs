// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey, Harshit Gindra. All rights reserved.
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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
    public interface IRulesFactory
    {
        /// <summary>
        /// Get all model rule instances registered based on TInterface
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns>list of IModelRule instances</returns>
        Task<IEnumerable<IRule<TInterface>>> GetAllAsync<TInterface>();

        /// <summary>
        /// Get all model rule instances registered based on TInterface and group name
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns>list of IModelRule instances</returns>
        Task<IEnumerable<IRule<TInterface>>> GetAllAsync<TInterface>(string group);
    }
}

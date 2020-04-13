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
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
    public interface IRule<TEntity>
    {
        /// <summary>
        /// Validate entity based on the defined rule asynchronously
        /// </summary>
        /// <param name="entity">TEntity to be validated</param>
        /// <returns>bool and consolidated error message</returns>
        Task<(bool, string)> ValidateAsync(TEntity entity);
        /// <summary>
        /// Group name to distinguish between different rules
        /// </summary>
        string Group { get; set; }
    }
}

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
    /// Interface defining a generic rule.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public interface IRule<TItem>
    {
        /// <summary>
        /// Group name to distinguish between different rule sets.
        /// </summary>
        string Group { get; set; }

        /// <summary>
        /// Validate entity based on the defined rule asynchronously.
        /// </summary>
        /// <param name="item">The item to be validated.</param>
        /// <returns>A boolean value indicating whether or not the rule has been
        /// validated. If false, an error message is returned.</returns>
        Task<(bool, string)> ValidateAsync(TItem item);
    }
}

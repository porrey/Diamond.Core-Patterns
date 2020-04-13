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
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Rules
{
    public class RulesNotFoundException<TItem>: DiamondPatternsException
    {
        public RulesNotFoundException()
            : base($"Rules of type 'IRule<{typeof(TItem).Name}>' has not been configured.")
        {
        }

        public RulesNotFoundException(string name)
            : base($"A Rule of type 'IRule<{typeof(TItem).Name}>' named '{name}' has not been configured.")
        {
        }
    }
}
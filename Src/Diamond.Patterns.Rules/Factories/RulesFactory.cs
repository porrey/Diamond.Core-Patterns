﻿// ***
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diamond.Patterns.Rules
{
    /// <summary>
    /// Defines a generic repository factory that can be used to retrieve
    /// an object that implements IModelRule<TItem, TResult> from the container.
    /// </summary>
    public class RulesFactory : IRulesFactory
    {
        public RulesFactory(IObjectFactory objectFactory)
        {
            this.ObjectFactory = objectFactory;
        }

        protected IObjectFactory ObjectFactory { get; set; }

        /// <summary>
        /// Get all model rule instances registered based on TInterface
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <returns>list of IModelRule instances</returns>
        public Task<IEnumerable<IRule<TItem>>> GetAllAsync<TItem>()
        {
            // ***
            // *** Get all model rules  from the container of
            // *** type IModelRule<TItem>.
            // ***
            IEnumerable<IRule<TItem>> instances = this.ObjectFactory.GetAllInstances<IRule<TItem>>();

            // ***
            // *** Make sure that there are rules registered
            // *** for the specified TItem and group
            // ***
            if (instances == null || !instances.Any())
            {
                throw new RulesNotFoundException<TItem>();
            }

            return Task.FromResult(instances);
        }
        /// <summary>
        /// Get all model rule instances registered based on TInterface and group name
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <returns>list of IModelRule instances</returns>
        public Task<IEnumerable<IRule<TItem>>> GetAllAsync<TItem>(string group)
        {
            // ***
            // *** Get all model rules  from the container of
            // *** type IModelRule<TItem> and group
            // ***
            IEnumerable<IRule<TItem>> instances = this.ObjectFactory.GetAllInstances<IRule<TItem>>()
                .Where(t => t.Group == group);

            // ***
            // *** Make sure that there are rules registered
            // *** for the specified TItem and group
            // ***
            if (instances == null || !instances.Any())
            {
                throw new RulesNotFoundException<TItem>();
            }
            
            return Task.FromResult(instances);
        }
    }
}

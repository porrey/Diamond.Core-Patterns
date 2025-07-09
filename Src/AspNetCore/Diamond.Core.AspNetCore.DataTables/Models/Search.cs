//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
using Newtonsoft.Json;

namespace Diamond.Core.AspNetCore.DataTables
{
    /// <summary>
    /// Represents search criteria for a data table, including the search value and whether to use regular expressions.
    /// </summary>
    public class Search : DataTablesObject
    {
        /// <summary>
        /// Gets or sets the search value to apply to the data table.
        /// </summary>
        [JsonProperty("value")]
        public virtual string Value { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the search value should be treated as a regular expression.
        /// </summary>
        [JsonProperty("regex")]
        public virtual bool RegEx { get; set; }
    }
}

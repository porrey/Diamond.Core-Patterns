﻿//
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
    /// Represents sorting information for a column in a data table.
    /// </summary>
    public class Order : DataTablesObject
    {
        /// <summary>
        /// Gets or sets the index of the column to which the sorting should be applied.
        /// </summary>
        [JsonProperty("column")]
        public virtual int Column { get; set; }

        /// <summary>
        /// Gets or sets the direction of the sort ("asc" or "desc").
        /// </summary>
        [JsonProperty("dir")]
        public virtual string Dir { get; set; }
    }
}

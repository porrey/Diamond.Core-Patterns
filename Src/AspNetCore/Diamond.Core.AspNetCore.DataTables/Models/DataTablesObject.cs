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
using Newtonsoft.Json.Linq;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Represents an abstract base class for objects that can hold additional data fields not explicitly defined in the
	/// class structure.
	/// </summary>
	/// <remarks>This class uses JSON extension data to store extra data fields in a dictionary. The <see
	/// cref="ExtraData"/> property allows for dynamic storage of additional key-value pairs, where the key is a string and
	/// the value is a <see cref="JToken"/>.</remarks>
	public abstract class DataTablesObject : IDataTablesObject
	{
		/// <summary>
		/// Gets or sets a dictionary that holds additional data fields not explicitly defined in the class structure.
		/// </summary>
		[JsonExtensionData]
		public IDictionary<string, JToken> ExtraData { get; set; } = new Dictionary<string, JToken>();
	}
}

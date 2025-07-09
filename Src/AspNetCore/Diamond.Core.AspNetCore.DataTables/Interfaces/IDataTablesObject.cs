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
using Newtonsoft.Json.Linq;

namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Represents an object that can hold additional data in a key-value pair format.
	/// </summary>
	/// <remarks>This interface is designed to be implemented by classes that require the ability to store extra
	/// data dynamically. The data is stored as a dictionary where the key is a string and the value is a <see
	/// cref="JToken"/>, allowing for flexible data types.</remarks>
	public interface IDataTablesObject
	{
		/// <summary>
		/// Gets or sets additional data associated with the object.
		/// </summary>
		IDictionary<string, JToken> ExtraData { get; set; }
	}
}
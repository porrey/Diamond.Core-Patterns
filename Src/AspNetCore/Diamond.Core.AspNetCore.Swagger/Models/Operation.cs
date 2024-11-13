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

namespace Diamond.Core.AspNetCore.Swagger
{
	/// <summary>
	/// Represents a JSON patch operation.
	/// </summary>
	public class Operation
	{
		/// <summary>
		/// JSON Pointer (IETF RFC 6901) defines a string format for identifying a specific value within a
		/// JSON document. It is used by all operations in JSON Patch to specify the part of the document
		/// to operate on.
		/// </summary>
		[JsonProperty("path")]
		public virtual string Path { get; set; }

		/// <summary>
		/// Get/sets the value applied in the operation.
		/// </summary>
		[JsonProperty("value")]
		public virtual object Value { get; set; }

		/// <summary>
		/// Gets  or sets the operation to perform. The value can be one of the following: Add, Remove, Replace, Copy,
		/// Move or Test.
		/// </summary>
		[JsonProperty("op")]
		public virtual string Op { get; set; }
	}
}
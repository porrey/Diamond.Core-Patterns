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
namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public class Alias
	{
		/// <summary>
		/// Gets or sets the key for the alias.
		/// </summary>
		public virtual string Key { get; set; }

		/// <summary>
		/// Gets or sets the type definition for the alias.
		/// </summary>
		public virtual string TypeDefinition { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Alias"/> class.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"{this.Key} => {this.TypeDefinition}";
	}
}

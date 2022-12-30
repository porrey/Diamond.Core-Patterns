//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using System;

namespace Diamond.Core.Workflow.State
{
	/// <summary>
	/// Defines a state converter that can convert a state dictionary time from one type to another.
	/// </summary>
	public interface IStateTypeConverter
	{
		/// <summary>
		/// Get the target type of the converter.
		/// </summary>
		Type TargetType { get; }

		/// <summary>
		/// Converts the item from the source to the target.
		/// </summary>
		/// <param name="sourceValue">The value being converted.</param>
		/// <param name="specificTargetType">The target type of the conversion.</param>
		/// <returns>Returns the converted item or an error.</returns>
		(bool, string, object) ConvertSource(object sourceValue, Type specificTargetType);
	}
}

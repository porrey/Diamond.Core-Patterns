//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.Abstractions;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Represents an error that occurs when an expected exception object is not found in the context properties.
	/// </summary>
	/// <remarks>This exception is typically thrown when an operation requires an exception object to be present in
	/// the context, but none is available. Use this exception to signal missing exception information in custom error
	/// handling scenarios.</remarks>
	public class NoExceptionException : WorkflowException
	{
		/// <summary>
		/// Initializes a new instance of the NoExceptionException class, indicating that an exception object was not found in
		/// the context properties.
		/// </summary>
		public NoExceptionException()
			: base($"An exception object was not found in the context properties.")
		{
		}
	}
}

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
	/// Represents an error that occurs when an expected exit code is missing from the context properties.
	/// </summary>
	/// <remarks>This exception is typically thrown when an operation requires an exit code to be present in the
	/// context, but none is found. Use this exception to signal that the exit code retrieval failed due to its
	/// absence.</remarks>
	public class NoExitCodeException : WorkflowException
	{
		/// <summary>
		/// Initializes a new instance of the NoExitCodeException class, indicating that an exit code was not found in the
		/// context properties.
		/// </summary>
		public NoExitCodeException()
			: base($"An exit code was not found in the context properties.")
		{
		}
	}
}

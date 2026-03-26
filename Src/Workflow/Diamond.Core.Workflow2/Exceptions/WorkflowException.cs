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
using System;
using Diamond.Core.Abstractions;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Represents errors that occur during workflow execution.
	/// </summary>
	/// <remarks>This is the base class for exceptions thrown by workflow-related operations. Use this type to catch
	/// and handle workflow-specific errors separately from other exception types. Derived classes provide more specific
	/// error information for various workflow failure scenarios.</remarks>
	public abstract class WorkflowException : DiamondCoreException
	{
		/// <summary>
		/// Initializes a new instance of the WorkflowException class.
		/// </summary>
		public WorkflowException()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the WorkflowException class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error. This value is passed to the base Exception class and can be accessed via the
		/// Message property.</param>
		public WorkflowException(string message)
				: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the WorkflowException class with a specified error message and a reference to the
		/// inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
		public WorkflowException(string message, Exception innerException) :
				base(message, innerException)
		{
		}
	}
}

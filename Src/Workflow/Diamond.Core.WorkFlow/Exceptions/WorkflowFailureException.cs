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

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Represents an exception that is thrown when a workflow fails at a specific step.
	/// </summary>
	/// <remarks>This exception provides information about the workflow step where the failure occurred, including
	/// the step name and number. The underlying cause of the failure is available in the inner exception.</remarks>
	public class WorkflowFailureException : WorkflowException
	{
		/// <summary>
		/// Initializes a new instance of the WorkflowFailureException class with a specified inner exception, step name, and
		/// step number indicating where the workflow failed.
		/// </summary>
		/// <remarks>Use this constructor to provide detailed context about workflow failures, including the specific
		/// step and underlying exception. The step information can help diagnose and resolve issues in complex
		/// workflows.</remarks>
		/// <param name="innerException">The exception that caused the workflow to fail. Cannot be null.</param>
		/// <param name="stepName">The name of the workflow step where the failure occurred.</param>
		/// <param name="stepNumber">The zero-based index of the workflow step where the failure occurred.</param>
		public WorkflowFailureException(Exception innerException, string stepName, int stepNumber)
			: base($"The work flow failed at step '{stepName}' [{stepNumber}]. See the inner exception for details.", innerException)
		{
		}
	}
}

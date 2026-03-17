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
namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Represents an exception that is thrown when a workflow step fails for an unknown reason.
	/// </summary>
	/// <remarks>This exception is typically used to indicate that a workflow step encountered an error that could
	/// not be classified or handled by more specific exception types. It provides information about the step name and step
	/// number to aid in troubleshooting.</remarks>
	public class UnknownFailureException : WorkflowException
	{
		/// <summary>
		/// Initializes a new instance of the UnknownFailureException class for a specific step that failed due to an unknown
		/// reason.
		/// </summary>
		/// <param name="stepName">The name of the step that encountered the failure.</param>
		/// <param name="stepNumber">The sequence number of the step that failed.</param>
		public UnknownFailureException(string stepName, int stepNumber)
			: base($"The step '{stepName}' [{stepNumber}] failed for an unknown reason.")
		{
		}
	}
}

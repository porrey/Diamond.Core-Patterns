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
	/// Represents an exception that is thrown when no workflow steps are found for the specified group.
	/// </summary>
	/// <remarks>This exception is typically used to indicate a configuration or runtime error in workflow
	/// processing when a required group of steps is missing. It derives from WorkflowException and provides a message
	/// identifying the missing group.</remarks>
	public class MissingStepsException : WorkflowException
	{
		/// <summary>
		/// Initializes a new instance of the MissingStepsException class with a specified workflow group name.
		/// </summary>
		/// <remarks>This exception is typically thrown when a workflow operation expects steps to be defined for a
		/// given group, but none are present. Use this exception to signal missing configuration or incomplete workflow
		/// definitions.</remarks>
		/// <param name="group">The name of the workflow group for which no steps were found. Cannot be null or empty.</param>
		public MissingStepsException(string group)
			: base($"No workflow steps with group '{group}' were found.")
		{
		}
	}
}

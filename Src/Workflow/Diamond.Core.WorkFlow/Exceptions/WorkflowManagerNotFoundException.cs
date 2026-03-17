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
	/// Represents an exception that is thrown when a workflow manager for a specified group name has not been configured.
	/// </summary>
	/// <remarks>This exception is typically thrown when attempting to access a workflow manager instance that does
	/// not exist for the given group name. Use this exception to identify configuration issues related to workflow manager
	/// registration.</remarks>
	public class WorkflowManagerNotFoundException : DiamondCoreException
	{
		/// <summary>
		/// Initializes a new instance of the WorkflowManagerNotFoundException class with the specified workflow manager group
		/// name.
		/// </summary>
		/// <param name="groupName">The name of the workflow manager group that was not found. Cannot be null or empty.</param>
		public WorkflowManagerNotFoundException(string groupName)
			: base($"A work flow manager of type 'IWorkflowManager<{typeof(IContext).Name}>' with group name '{groupName}' has not been configured.")
		{
		}
	}
}

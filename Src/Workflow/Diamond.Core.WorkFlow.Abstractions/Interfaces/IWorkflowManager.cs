﻿//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
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
using System.Threading.Tasks;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Defines a generic work flow manager.
	/// </summary>
	public interface IWorkflowManager
	{
		/// <summary>
		/// The group name used to determine the work flow
		/// items that are part of this work flow.
		/// </summary>
		string Group { get; set; }

		/// <summary>
		/// 
		/// </summary>
		IWorkflowItemFactory WorkflowItemFactory { get; set; }

		/// <summary>
		/// Gets the work flow items in their execution order.
		/// </summary>
		IWorkflowItem[] Steps { get; }

		/// <summary>
		/// Executes the work flow.
		/// </summary>
		/// <param name="context">The current context to be used for this instance of the work flow execution.</param>
		/// <returns>True if the work flow was successful; false otherwise.</returns>
		Task<bool> ExecuteWorkflowAsync(IContext context);
	}
}
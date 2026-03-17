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
using System.Threading.Tasks;
using Diamond.Core.Workflow.State;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Defines a generic context that can be used for a workflow. The
	/// context allows data to be shared between the multiple steps of
	/// a workflow during execution.
	/// </summary>
	public class WorkflowContext : DisposableObject, IContext
	{
		/// <summary>
		/// Gets or sets the name associated with this instance. The name is used for logging.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// Gets or sets the collection of command-line arguments associated with the current operation.
		/// </summary>
		public virtual string[] Arguments { get; set; }

		/// <summary>
		/// Gets the collection of state properties associated with the current context.
		/// </summary>
		/// <remarks>Use this property to store and retrieve custom data relevant to the context. The collection
		/// persists for the lifetime of the context and can be used to share information between components.</remarks>
		public virtual IStateDictionary Properties { get; } = new StateDictionary();

		/// <summary>
		/// Resets the current instance asynchronously to its initial state.
		/// </summary>
		/// <returns>A task that represents the asynchronous reset operation.</returns>
		/// <exception cref="NotImplementedException">Thrown if the method is not implemented in a derived class.</exception>
		public virtual Task ResetAsync()
		{
			throw new NotImplementedException();
		}
	}
}

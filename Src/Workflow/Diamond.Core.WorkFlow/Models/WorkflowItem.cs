//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// This should not be used. Use <see cref="WorkflowItemTemplate"/> instead.
	/// </summary>
	[Obsolete("Please use WorkflowItemTemplate instead.")]
	public abstract class WorkflowItem : WorkflowItemTemplate
	{
		/// <summary>
		/// 
		/// </summary>
		public WorkflowItem()
		{
			this.Name = this.GetType().Name.Replace("Step", "");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public WorkflowItem(ILogger<WorkflowItemTemplate> logger)
			: base()
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="name"></param>
		/// <param name="group"></param>
		/// <param name="ordinal"></param>
		public WorkflowItem(ILogger<WorkflowItemTemplate> logger, string name, string group, int ordinal)
			: base(logger)
		{
			this.Name = name;
			this.Group = group;
			this.Ordinal = ordinal;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="name"></param>
		/// <param name="group"></param>
		/// <param name="ordinal"></param>
		/// <param name="alwaysExecute"></param>
		public WorkflowItem(ILogger<WorkflowItemTemplate> logger, string name, string group, int ordinal, bool alwaysExecute)
			: base(logger, name, group, ordinal)
		{
			this.AlwaysExecute = alwaysExecute;
		}

	}
}

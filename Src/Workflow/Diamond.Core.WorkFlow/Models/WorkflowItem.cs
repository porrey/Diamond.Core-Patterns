//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class WorkflowItem : IWorkflowItem
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
		public WorkflowItem(ILogger<WorkflowItem> logger)
			: this()
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
		public WorkflowItem(ILogger<WorkflowItem> logger, string name, string group, int ordinal)
			: this(logger)
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
		public WorkflowItem(ILogger<WorkflowItem> logger, string name, string group, int ordinal, bool alwaysExecute)
			: this(logger, name, group, ordinal)
		{
			this.AlwaysExecute = alwaysExecute;
		}

		/// <summary>
		/// Gets/sets the name of this workflow item for logging purposes.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// Gets/sets the group this item belongs to. Items are grouped together
		/// so that the WorkflowManager can gather the steps into a workable series.
		/// </summary>
		public virtual string Group { get; set; }

		/// <summary>
		/// The order this item appears in the execution steps.
		/// </summary>
		public virtual int Ordinal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual bool AlwaysExecute { get; set; } = false;

		/// <summary>
		/// 
		/// </summary>
		public virtual double Weight { get; set; } = 1;

		/// <summary>
		/// 
		/// </summary>
		public virtual ILogger<WorkflowItem> Logger { get; set; } = new NullLogger<WorkflowItem>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual bool ShouldExecute(IContext context)
		{
			return this.OnShouldExecuteAsync(context).Result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual Task<bool> OnShouldExecuteAsync(IContext context)
		{
			return Task.FromResult(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual async Task<bool> ExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			this.Logger.LogDebug($"Work Flow Step '{this.Name}': {nameof(ExecuteStepAsync)}");

			if (await this.OnPrepareForExecutionAsync(context))
			{
				returnValue = await this.OnExecuteStepAsync(context);
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected virtual Task<bool> OnPrepareForExecutionAsync(IContext context)
		{
			this.Logger.LogDebug($"Work Flow Step '{this.Name}': {nameof(OnPrepareForExecutionAsync)}");
			return Task.FromResult(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected virtual Task<bool> OnExecuteStepAsync(IContext context)
		{
			this.Logger.LogDebug($"Work Flow Step '{this.Name}': {nameof(OnExecuteStepAsync)}");
			return Task.FromResult(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		protected virtual Task StepFailedAsync(IContext context, string message)
		{
			this.Logger.LogDebug($"Work Flow Step '{this.Name}': {nameof(StepFailedAsync)}");
			context.SetException(message);
			return Task.FromResult(0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"[{this.Ordinal}] {this.Name} | Group: {this.Group}";
		}
	}
}

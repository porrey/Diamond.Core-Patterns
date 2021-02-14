//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.Extensions.InterfaceInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.WorkFlow {
	/// <summary>
	/// 
	/// </summary>
	public abstract class WorkFlowItem : IWorkFlowItem, ILoggerPublisher<WorkFlowItem> {
		/// <summary>
		/// Gets/sets the name of this work-flow item for logging purposes.
		/// </summary>
		public virtual string Name { get; set; } = "Name Not Set";

		/// <summary>
		/// Gets/sets the group this item belongs to. Items are grouped together
		/// so that the WorkFlowManager can gather the steps into a workable series.
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
		public double Weight { get; set; } = 1;

		/// <summary>
		/// 
		/// </summary>
		public ILogger<WorkFlowItem> Logger { get; set; } = new NullLogger<WorkFlowItem>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual bool ShouldExecute(IContext context) {
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual async Task<bool> ExecuteStepAsync(IContext context) {
			bool returnValue = false;

			this.Logger.LogDebug($"Work Flow Step '{this.Name}': {nameof(ExecuteStepAsync)}");

			if (await this.OnPrepareForExecutionAsync(context)) {
				returnValue = await this.OnExecuteStepAsync(context);
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected virtual Task<bool> OnPrepareForExecutionAsync(IContext context) {
			this.Logger.LogDebug($"Work Flow Step '{this.Name}': {nameof(OnPrepareForExecutionAsync)}");
			return Task.FromResult(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected virtual Task<bool> OnExecuteStepAsync(IContext context) {
			this.Logger.LogDebug($"Work Flow Step '{this.Name}': {nameof(OnExecuteStepAsync)}");
			return Task.FromResult(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return $"[{this.Ordinal}] {this.Name} | Group: {this.Group}";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		protected Task StepFailedAsync(IContext context, string message) {
			this.Logger.LogDebug($"Work Flow Step '{this.Name}': {nameof(StepFailedAsync)}");
			context.SetException(message);
			return Task.FromResult(0);
		}
	}
}
